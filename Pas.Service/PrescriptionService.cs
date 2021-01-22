﻿using Microsoft.EntityFrameworkCore;
using Pas.Common.Constants;
using Pas.Common.Enums;
using Pas.Data;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pas.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ICacheService _cacheService;

        public PasContext _pasContext { get; }

        public PrescriptionService(PasContext PasContext, ICacheService CacheService)
        {
            _pasContext = PasContext;
            _cacheService = CacheService;
        }

        /// <summary>
        /// Once Doctor selects a Patient- will initiate a new Prescription.
        /// This will insert a Record in the Prescription table- with PatientId, DoctorId and Hospital Id.
        /// </summary>
        /// <param name="vm">PrescriptionCreateInitialVM ViewModel</param>
        /// <returns>Newly created Prescription Id</returns>
        public async Task<int> CreateInitialDefault(PrescriptionCreateInitialVM vm)
        {
            //## Find the Doctor- Chamber info
            var doctorChamber = await _pasContext.UserOrganisationRole
                                            .FirstOrDefaultAsync(or=> or.RoleId == (int)ApplicationRole.Doctor
                                                                && or.OrganisationId== vm.HospitalId
                                                                && or.UserId == vm.DoctorId);
            
            var newPrescription = new Prescription()
            {
                DoctorId = vm.DoctorId,
                HospitalId = vm.HospitalId,
                PatientId = vm.PatientId,
            };

            await _pasContext.Prescription.AddAsync(newPrescription);
            await _pasContext.SaveChangesAsync();

            return newPrescription.Id;
        }

        public Task<int> AddDrugToPrescription(PrescriptionDrugCreateVM prescriptionDrug)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddTestToPrescription(PrescriptionDiagnosticTestCreateVM prescriptionDiagnosticTest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(PrescriptionConfirmSaveVM prescriptionUpdateVM)
        {
            throw new NotImplementedException();
        }

        public async Task<Prescription> Find(int id)
        {
            string cacheKey = $"Prescription_{id}";
            var prescription = _cacheService.GetCacheValue<Prescription>(cacheKey);

            if (prescription is null) {
                prescription = await _pasContext.Prescription.FindAsync(id);
            
                _cacheService.SetCacheValue(cacheKey, prescription);
            }


            return prescription;
        }

        public async Task<IEnumerable<PrescriptionViewVM>> ListByPatient(int id)
        {

            string cacheKey = $"prescriptionList_{id}";
            var prescriptionList = _cacheService.GetCacheValue<List<PrescriptionViewVM>>(cacheKey);

            if (prescriptionList is null)
            {
                prescriptionList = await _pasContext.Prescription
                                        .Include(p => p.Doctor)
                                        .Include(p => p.Hospital)
                                        .Include(p => p.PrescriptionDrugs)
                                        .AsNoTracking()
                                        .Where(p => p.PatientId == id)
                                        .Select(p=> new PrescriptionViewVM() { 
                                            Id = p.Id,
                                            DateCreated = p.DateCreated,
                                            DoctorId = p.DoctorId,
                                            DoctorName = $"{p.Doctor.FirstName} {p.Doctor.LastName}",
                                            HospitalId = p.HospitalId,
                                            HospitalName = p.Hospital.Name
                                        })
                                        .ToListAsync();



                _cacheService.SetCacheValue(cacheKey, prescriptionList);
            }

            

            return prescriptionList;
        }


        /// <summary>This will Insert a New drug Item in the Prescription</summary>
        /// <param name="vm">PrescriptionDrugCreateVM View Model</param>
        /// <returns>A String- semi-colon separated value</returns>
        public async Task<string> Insert_PescriptionItem(PrescriptionDrugCreateVM vm)
        {
            //## First insert this new Prescription Item in the table
            var newPrescriptionItem = new PrescriptionDrugs()
            {
                PrescriptionId = vm.PrescriptionId,
                DrugBrandId = vm.DrugBrandId,
                BrandDoseTemplateId = vm.BrandDoseTemplateId,
                //Notes = vm.Notes
            };

            if (vm.AdviseInstructionId > 0)
                newPrescriptionItem.AdviseInstructionId = vm.AdviseInstructionId;

            await _pasContext.PrescriptionDrugs.AddAsync(newPrescriptionItem);
            await _pasContext.SaveChangesAsync();



            //## Now get the Intake Pattern from Pattern table- to return to UI- to show in the Prescription Preview
            var newItemId = newPrescriptionItem.Id;

            var details = await _pasContext.BrandDoseTemplates
                                                    .Include(bd => bd.IntakePattern)
                                                    .AsNoTracking()
                                                    .Where(bd => bd.Id == vm.BrandDoseTemplateId)
                                                    .Select(bd => new PrescriptionDrugViewVM()
                                                    {
                                                        PrescriptionItemId = newItemId,             //## Will need this to Delete this Item- if required
                                                        IntakeTemplate = bd.IntakePattern.Pattern,  //## this could be in Bangla or English                                                      
                                                    })
                                                    .FirstOrDefaultAsync();

            string result = $"{newItemId};{details.IntakeTemplate}";

            return result;
        }


        public async Task<bool> Delete_PescriptionItem(int prescriptionItemId)
        {
            try
            {
                PrescriptionDrugs drugItem = new PrescriptionDrugs() { Id = prescriptionItemId };
                _pasContext.Remove(drugItem);
                await _pasContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> Prescription_FinishAndCreate_HTML(PrescriptionConfirmSaveAndFinishVM vm)
        {
            string filePath = $"C:\\Shawkat\\dummy\\prescription\\p-{vm.PrescriptionId}.html";
            try
            {
                //## Chief Complaints - INSERT
                foreach (var item in vm.ccList)
                {
                    PrescriptionChiefComplaints cc = new PrescriptionChiefComplaints() { 
                        PrescriptionId = vm.PrescriptionId,
                        PatientId= vm.PatientId,
                        ChiefComplaintId = item
                    };

                    _pasContext.PrescriptionChiefComplaints.Add(cc);
                }

                //## Indications/ Diagnosis
                var diagnosisList = vm.Diagnosis.Split(",", StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in diagnosisList)
                {
                    int diagnosisId = Convert.ToInt32(item);
                    PatientIndications pi = new PatientIndications()
                    {
                        PrescriptionId = vm.PrescriptionId,
                        PatientId = vm.PatientId,
                        IndicationTypeId = diagnosisId
                    };

                    _pasContext.PatientIndications.Add(pi);
                }



                //##    LabTestRequestList
                var labTestRequestList = vm.LabTestRequestList.Split(",", StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in labTestRequestList)
                {
                    int testId = Convert.ToInt32(item);
                    PrescriptionDiagnosticTest pdt = new PrescriptionDiagnosticTest()
                    {
                        PrescriptionId = vm.PrescriptionId,                    
                        DiagnosticTestId = testId
                    };

                    _pasContext.PrescriptionDiagnosticTest.Add(pdt);
                }

                Prescription prescription = await _pasContext.Prescription.FindAsync(vm.PrescriptionId);
                prescription.Status = (int)PrescriptionStatus.Complete;
                prescription.IsRepeatingVisit = vm.IsFollowUpVisit;
                prescription.Notes = vm.Notes;
                prescription.Plans = vm.Plans;
                prescription.Advise = vm.Advise;
                //prescription.ReferralDoctor = vm.ReferralDoctor;
                prescription.DateCreated = DateTime.Now;

                _pasContext.Prescription.Update(prescription);
                
                await _pasContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            using (StreamWriter writer = System.IO.File.CreateText(filePath))
            {
                writer.WriteLine(vm.HtmlContents);
            }

            return await Task.FromResult(true);
        }

        public async Task<string> GetPrescription_HTML(int id)
        {
            string path = $"C:\\Shawkat\\dummy\\prescription\\p-{id}.html";
            string content = System.IO.File.ReadAllText(path);

            return await Task.FromResult(content);
        }

        public async Task<int> Update_Vitals(VitalsVM vm)
        {
            if (vm.PatientId < 1) return await Task.FromResult(0);

            VitalsHistory vh = new VitalsHistory();

            //## Its Update or INSERT
            if (vm.Id > 0) {
                vh = _pasContext.VitalsHistories.Find(vm.Id);
                vh.BloodPulse = vm.BloodPulse;
                vh.Temperature = vm.Temperature;
                vh.Diastolic = vm.Diastolic;
                vh.Systolic = vm.Systolic;
                vh.Weight = vm.Weight;

                _pasContext.Update(vh);
                await _pasContext.SaveChangesAsync();
            }
            else
            {
                try
                {
                    vh = new VitalsHistory()
                    {
                        Id= 0,
                        PatientId = vm.PatientId,
                        PrescriptionId = vm.PrescriptionId,
                        Temperature = vm.Temperature,
                        BloodPulse = vm.BloodPulse,
                        Systolic = vm.Systolic,
                        Diastolic = vm.Diastolic,
                        Weight = vm.Weight,
                        DateAdded = DateTime.Now
                    };

                    await _pasContext.VitalsHistories.AddAsync(vh);
                    await _pasContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }

            }

            //## Update the Redis Cache for next read
            string redisKey = $"{CacheKey.PatientVitals}_{vm.PatientId}";
            _cacheService.SetCacheValue(redisKey, vm);

            return vh.Id;

        }

        public async Task<int> Insert_PrescriptionExaminationItem(PrescriptionExaminationItemVM vm)
        {
            var newExamination = new PrescriptionExamination() 
            {
                PrescriptionId = vm.PrescriptionId,
                ExaminationItemId = vm.ExaminationId,
                ExaminationItemOptionId = vm.ExaminationItemOptionId,
                Result = vm.Findings
            };
            try
            {
                await _pasContext.PrescriptionExaminations.AddAsync(newExamination);
                await _pasContext.SaveChangesAsync();

                return newExamination.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }

        }

        public async Task<bool> Delete_PrescriptionExaminationItem(int id)
        {
            var examItem = new PrescriptionExamination() { Id = id };
            _pasContext.Remove(examItem);
            await _pasContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<InvestigationVM>> ListAll_Investigations()
        {
            var cachedResult = _cacheService.GetCacheValue<IEnumerable<InvestigationVM>>(CacheKey.Investigations);

            if (cachedResult is null) {
                cachedResult = await _pasContext.Investigation.Select(i=> new InvestigationVM()
                {
                    Id = i.Id,
                    Description = i.Description,
                    ParentId = i.ParentId
                })
                    .OrderBy(i=> i.Description)
                    .ToListAsync()
                ;

                _cacheService.SetCacheValue(CacheKey.Investigations, cachedResult);
            }

            return cachedResult;
        }

        public async Task<IList<InvestigationVM>> ListAllInvestigationChildItems(int id)
        {
            var cachedResult = _cacheService.GetCacheValue<List<InvestigationVM>>(CacheKey.Investigations);
            
            if (cachedResult is null) {
                cachedResult = await _pasContext.Investigation
                                                    .AsNoTracking()
                                                    //.Where(i => i.ParentId == id)
                                                    .Select(i => new InvestigationVM() 
                                                    {
                                                        Id = i.Id,
                                                        Description = i.Description,
                                                        ParentId = i.ParentId
                                                    }).ToListAsync();

                _cacheService.SetCacheValue(CacheKey.Investigations, cachedResult);
                    
            }

            //## Now take all the Children of that ParentId
            var filteredList = cachedResult
                                    .Where(c => c.ParentId == id)
                                    .ToList();

            return filteredList;
        }

        public async Task<int> Insert_InvestigationItem(PrescriptionInvestigationVM vm)
        {
            PrescriptionInvestigation pv = new PrescriptionInvestigation() { 
                InvestigationId = vm.InvestigationId,
                PrescriptionId = vm.PrescriptionId,
                Notes = vm.Notes
            };

            await _pasContext.PrescriptionInvestigations.AddAsync(pv);
            await _pasContext.SaveChangesAsync();

            return pv.Id;
        }

        public async Task<bool> Delete_InvestigationItem(int investiogtionItemId)
        {
            var pv = new PrescriptionInvestigation() { Id = investiogtionItemId };

            _pasContext.Remove(pv);
            await _pasContext.SaveChangesAsync();

            return true;


        }
        
    }
}
