using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DataContracts
{
    public interface ICompanyData
    {
        IEnumerable<Company> GetCompanies();
        Company GetCompanyById(int Id);
        Company GetCompanyByName(string companyName);    
        bool CreateCompany(Company company);
        bool UpdateCompany(Company company, int Id);
        bool DeleteCompany(int Id);
        bool DisableCompany(int Id);

        IList<TalukaDetails> GetTalukaDetails(int divisionId);
        IList<DivisionDetails> GetDivisionDetails(int stateId);
        IList<StateDetails> GetStateDetails(int countryId);
    }
}
