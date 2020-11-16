namespace Pas.Service.Interface
{
    public interface IUniquePatientCodeGenerator
    {
        /// <summary>
        /// This will generate a Unique key for a Patient- in the PAS app
        /// </summary>
        /// <param name="mobileNumber">Full Mobile number</param>
        /// <param name="districtId">District Id</param>
        /// <returns></returns>
        string Get(string mobileNumber, int districtId);
    }
}