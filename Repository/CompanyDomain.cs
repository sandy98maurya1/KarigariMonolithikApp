using Contract;
using Contract.DataContracts;
using Models;
using System.Collections.Generic;

namespace Domain
{
    public class CompanyDomain : ICompany
    {
        private readonly ICompanyData companyData;
        public CompanyDomain(ICompanyData _company)
        {
            companyData = _company;
        }
        public bool CreateCompany(Company company)
        {
            return companyData.CreateCompany(company);
        }

        public bool DeleteCompany(int Id)
        {
            return companyData.DeleteCompany(Id);
        }

        public bool DisableCompany(int Id)
        {
            return companyData.DisableCompany(Id);

        }

        public IEnumerable<Company> GetCompanies()
        {
            return companyData.GetCompanies();
        }

        public Company GetCompanyById(int Id)
        {
            return companyData.GetCompanyById(Id);
        }

        public Company GetCompanyByName(string companyName)
        {
            return companyData.GetCompanyByName(companyName);
        }

        public bool UpdateCompany(Company company, int id)
        {
            return companyData.UpdateCompany(company,  id);
        }

        public IList<DivisionDetails> GetDivisionDetails(int stateId) => companyData.GetDivisionDetails(stateId);

        public IList<StateDetails> GetStateDetails(int countryId) => companyData.GetStateDetails(countryId);

        public IList<TalukaDetails> GetTalukaDetails(int divisionId) => companyData.GetTalukaDetails(divisionId);

    }
}
