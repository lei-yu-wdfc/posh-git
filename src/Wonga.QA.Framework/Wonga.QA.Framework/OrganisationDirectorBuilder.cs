using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public class OrganisationDirectorBuilder
    {
        private Guid _id;
        private Customer _customer;
        private Organisation _company;
        private String _email;
        private String _foreName;
        private String _surname;
        private bool _primaryLevel;

        private OrganisationDirectorBuilder()
        {
            _id = Guid.NewGuid();
            _email = Data.GetEmail();
            _foreName = Data.GetName();
            _surname = Data.GetName();
            _primaryLevel = true;
        }

        public static OrganisationDirectorBuilder New(Customer customer, Organisation company)
        {
            return new OrganisationDirectorBuilder { _customer = customer, _company = company };
        }

        public OrganisationDirectorBuilder WithEmail(String email)
        {
            _email = email;
            return this;
        }

        public OrganisationDirectorBuilder WithForename(String forename)
        {
            _foreName = forename;
            return this;
        }

        public OrganisationDirectorBuilder WithSurname(String surname)
        {
            _surname = surname;
            return this;
        }

        public OrganisationDirectorBuilder AsPrimaryDirector()
        {
            _primaryLevel = true;
            return this;
        }

        public OrganisationDirectorBuilder AsSecondaryDirector()
        {
            _primaryLevel = false;
            return this;
        }

        public OrganisationDirector Build()
        {
            if (_primaryLevel)
            {
                Driver.Api.Commands.Post(AddPrimaryOrganisationDirectorCommand.New(r =>
                                                                                       {                                                                                           
                                                                                           r.OrganisationId =
                                                                                               _company.Id;
                                                                                           r.AccountId = _customer.Id;
                                                                                           r.Email = _email;
                                                                                           r.Forename = _foreName;
                                                                                           r.Surname = _surname;
                                                                                       }));
            }
            else
            {
                Driver.Api.Commands.Post(AddSecondaryOrganisationDirectorCommand.New(r =>
                {
                    r.OrganisationId =
                        _company.Id;
                    r.AccountId = _customer.Id;
                    r.Email = _email;
                    r.Forename = _foreName;
                    r.Surname = _surname;
                }));
            }

            return new OrganisationDirector(_customer.Id);
        }
    }
}
