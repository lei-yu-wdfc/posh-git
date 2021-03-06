﻿using System;
using Wonga.QA.Framework.Api.Requests.Comms.ContactManagement.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;

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
            _email = Get.RandomEmail();
            _foreName = Get.GetName();
            _surname = Get.GetName();
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
                Drive.Api.Commands.Post(AddPrimaryOrganisationDirectorCommand.New(r =>
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
                Drive.Api.Commands.Post(AddSecondaryOrganisationDirectorCommand.New(r =>
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
