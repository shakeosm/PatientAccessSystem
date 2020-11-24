using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Prescription>> ListByPatient(int id)
        {

            string cacheKey = $"prescriptionList_{id}";
            var prescriptionList = _cacheService.GetCacheValue<List<Prescription>>(cacheKey);

            if (prescriptionList is null)
            {
                prescriptionList = await _pasContext.Prescription
                                        .Include(p => p.Doctor)
                                        .Include(p => p.Hospital)
                                        .Include(p => p.PrescriptionDrugs)
                                        .Where(p => p.PatientId == id).ToListAsync();

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

        public async Task<bool> Create_Prescription_HTML(int prescriptionId, string prescriptionContents)
        {
            string filePath = $"C:\\Shawkat\\dummy\\prescription\\p-{prescriptionId}.html";
            using (StreamWriter writer = System.IO.File.CreateText(filePath))
            {
                writer.WriteLine(prescriptionContents);
            }

            return await Task.FromResult(true);
        }

        public async Task<string> GetPrescription_HTML(int id)
        {
            string path = $"C:\\Shawkat\\dummy\\prescription\\p-{id}.html";
            string content = System.IO.File.ReadAllText(path);

            return await Task.FromResult(content);
        }

        //private async Task<PrescriptionAddVM> MapToCreateViewModel(Prescription prescription)
        //{
        //    var doctor = await _pasContext.UserOrganisationRole.FindAsync(prescription.DoctorOrgRoleId);

        //    return new PrescriptionAddVM()
        //    {
        //        DoctorId = doctor.UserId,
        //        PatientId = prescription.PatientId,
        //        HospitalId = doctor.OrganisationId
        //    };
        //}
    }
}
