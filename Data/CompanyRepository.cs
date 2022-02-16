
using Contract.DataContracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class CompanyRepository : ICompanyData
    {
        IConfiguration configuration;
        internal string connection { get; set; }
        public CompanyRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connection = ApplicationDbContext.GetConnection(configuration);
        }
        public IEnumerable<Company> GetCompanies()
        {
            List<Company> products = new List<Company>();

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"select C.Id, C.CompanyName, C.CompanyType, C.CompanyOwner, C.ContactPerson, C.ContactNo, C.CompanyEmail, C.Password, C.BusinessContactNo, C.AddressId, C.Role," +
                                "C.IsActive, C.RefreshToken, C.disabled, A.Id as CompanyAdd, A.Village, A.Taluka, A.City, A.State, A.Country, A.Zip, A.CompanyId From Company C, Company_Address A where C.ID= A.CompanyId";
                    products = dbConnection.Query<Company, Address, Company>(query, MapResults, splitOn: "CompanyAdd").ToList();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return products;
            }
        }
        public Company GetCompanyById(int companyId)
        {
            Company user = new Company();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"select C.Id, C.CompanyName, C.CompanyType, C.CompanyOwner, C.ContactPerson, C.ContactNo, C.CompanyEmail, C.Password, C.BusinessContactNo, C.AddressId, C.Role," +
                                "C.IsActive, C.RefreshToken, C.disabled, A.Id as CompanyAdd, A.Village, A.Taluka, A.City, A.State, A.Country, A.Zip, A.CompanyId From Company C, Company_Address A where C.ID= A.CompanyId and C.ID = @id";
                    user = (Company)dbConnection.Query<Company, Address, Company>(query, MapResults, new { @id = companyId }, splitOn: "CompanyAdd");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return user;
            }
        }
        public Company GetCompanyByName(string companyName)
        {
            Company user = new Company();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"select C.Id, C.CompanyName, C.CompanyType, C.CompanyOwner, C.ContactPerson, C.ContactNo, C.CompanyEmail, C.Password, C.BusinessContactNo, C.AddressId, C.Role," +
                                "C.IsActive, C.RefreshToken, C.disabled, A.Id as CompanyAdd, A.Village, A.Taluka, A.City, A.State, A.Country, A.Zip, A.CompanyId From Company C, Company_Address A where C.ID= A.CompanyId and C.CompanyName = @CompanyName";

                    user = (Company)dbConnection.Query<Company, Address, Company>(query, MapResults, new { @CompanyName = companyName }, splitOn: "UserId");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return user;
            }
        }


        public bool CreateCompany(Company company)
        {
            int count = 0;
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        var companyid = dbConnection.Query<int>(@"INSERT INTO Company(CompanyName,CompanyType,CompanyOwner,ContactPerson,ContactNo,CompanyEmail,Password,BusinessContactNo,Role,IsActive,RefreshToken,Disabled) VALUES (@CompanyName,@CompanyType,@CompanyOwner,@ContactPerson,@ContactNo,@CompanyEmail,@Password,@BusinessContactNo,@Role,@IsActive,@RefreshToken,@Disabled); SELECT CAST(SCOPE_IDENTITY() as INT);",
                        new { @CompanyName = company.CompanyName, @CompanyType = company.CompanyType, @CompanyOwner = company.CompanyOwner, @ContactPerson = company.ContactPerson, @ContactNo = company.ContactNo, @CompanyEmail = company.CompanyEmail, @Password = company.Password, @BusinessContactNo = company.BusinessContactNo, @Role = company.Role, @IsActive = company.IsActive, @RefreshToken = company.RefreshToken, @Disabled = company.disabled }, transaction: transaction).FirstOrDefault();
                        if (company.Address != null && companyid != 0)
                        {
                            count = dbConnection.Execute(@"INSERT INTO Company_Address(Village, Taluka, City, State, Country, Zip, CompanyId) 
                            VALUES(@Village, @Taluka, @City, @State, @Country, @Zip, @CompanyId);",
                            new { @Village = company.Address.Village, @Taluka = company.Address.Taluka, @City = company.Address.City, @State = company.Address.State, @Country = company.Address.Country, @Zip = company.Address.Zip, @CompanyId = companyid }, transaction: transaction);
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return count == 1 ? true : false;
            }
        }
        public bool UpdateCompany(Company company, int id)
        {
            int count = 0;
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        dbConnection.Query<int>(@"UPDATE Company set CompanyName=@CompanyName ,CompanyType=CompanyType,CompanyOwner=@CompanyOwner,ContactPerson=@ContactPerson,ContactNo=@ContactNo,CompanyEmail=@CompanyEmail,Password=@Password,BusinessContactNo=@BusinessContactNo,Role=@Role,IsActive=@IsActive,RefreshToken=@RefreshToken,Disabled=@Disabled Where ID=@iD",
                                                new { @CompanyName = company.CompanyName, @CompanyType = company.CompanyType, @CompanyOwner = company.CompanyOwner, @ContactPerson = company.ContactPerson, @ContactNo = company.ContactNo, @CompanyEmail = company.CompanyEmail, @Password = company.Password, @BusinessContactNo = company.BusinessContactNo, @Role = company.Role, @IsActive = company.IsActive, @RefreshToken = company.RefreshToken, @Disabled = company.disabled,@id=id}, transaction: transaction).FirstOrDefault();
                        if (company.Address != null && id != 0)
                        {
                            count = dbConnection.Execute(@"UPDATE Company_Address set Village=@Village, Taluka=@Taluka, City=@City, State=@State, Country=@Country, Zip=@Zip, CompanyId=@CompanyId", 
                            new { @Village = company.Address.Village, @Taluka = company.Address.Taluka, @City = company.Address.City, @State = company.Address.State, @Country = company.Address.Country, @Zip = company.Address.Zip, @CompanyId = id }, transaction: transaction);
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return count == 1 ? true : false;
            }
        }
        public bool DeleteCompany(int companyId)
        {
            var count = 0;

            using (var con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    var query = "update Company set Disabled=0 WHERE Id =@Id";
                    count = con.Execute(query, new { @Id = companyId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count == 1 ? true : false;
            }
        }

        public bool DisableCompany(int companyId)
        {
            var count = 0;

            using (var con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    var query = "update Users set IsActive=0 WHERE Id =@Id";
                    count = con.Execute(query, new { @Id = companyId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count == 1 ? true : false;
            }

        }
        private Company MapResults(Company user, Address address)
        {
            user.Address = address;
            return user;
        }


        public IList<StateDetails> GetStateDetails(int countryId)
        {
            IList<StateDetails> state = new List<StateDetails>();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,Name FROM [dbo].[Address_State]";

                    state = (List<StateDetails>)dbConnection.Query<StateDetails>(query);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return state;
            }
        }


        public IList<DivisionDetails> GetDivisionDetails(int stateId)
        {
            IList<DivisionDetails> divisions = new List<DivisionDetails>();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,Name, State_Id as StateId FROM [dbo].[Address_Division] where State_Id= @Id";

                    divisions = (List<DivisionDetails>)dbConnection.Query<DivisionDetails>(query, new { @Id = stateId });

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return divisions;
            }
        }


        public IList<TalukaDetails> GetTalukaDetails(int divisionId)
        {
            IList<TalukaDetails> talukaDetails = new List<TalukaDetails>();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,Name,  Division_ID as DivisionId FROM [dbo].[Address_Districts_Taluka] where Division_ID= @Id";

                    talukaDetails = (IList<TalukaDetails>)dbConnection.Query<TalukaDetails>(query, new { @Id = divisionId });

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return talukaDetails;
            }
        }

    }
}