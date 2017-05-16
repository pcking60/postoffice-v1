﻿using PostOffice.Model.Models;
using PostOffice.Web.Models;
using System;

namespace PostOffice.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
        }

        public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.POID = appUserViewModel.POID;
            appUser.Status = appUserViewModel.Status;
        }

        public static void UpdateDistrict(this District dis, DistrictViewModel vm) {
            dis.Code = vm.Code;
            dis.CreatedBy = vm.CreatedBy;
            dis.CreatedDate = vm.CreatedDate;
            dis.ID = vm.ID;
            dis.MetaDescription = vm.MetaDescription;
            dis.MetaKeyWord = vm.MetaKeyWord;
            dis.Name = vm.Name;
            dis.Status = vm.Status;
            dis.UpdatedBy = vm.UpdatedBy;
            dis.UpdatedDate = vm.UpdatedDate;
        }

        public static void UpdatePO(this PO po, POViewModel vm)
        {
            po.Code = vm.Code;
            po.CreatedBy = vm.CreatedBy;
            po.CreatedDate = vm.CreatedDate;
            po.DistrictID = vm.DistrictID;
            po.ID = vm.ID;
            po.MetaDescription = vm.MetaDescription;
            po.MetaKeyWord = vm.MetaKeyWord;
            po.Name = vm.Name;
            po.POAddress = vm.POAddress;
            po.POMobile = vm.POMobile;
            po.POStyle = vm.POStyle;
            po.UpdatedBy = vm.UpdatedBy;
            po.UpdatedDate = vm.UpdatedDate;
            po.Status = vm.Status;
          
        }

        public static void UpdateServiceGroup(this ServiceGroup serviceGroup, ServiceGroupViewModel serviceGroupVM)
        {
            serviceGroup.ID = serviceGroupVM.ID;
            serviceGroup.Alias = serviceGroupVM.Alias;
            serviceGroup.CreatedBy = serviceGroupVM.CreatedBy;
            serviceGroup.CreatedDate = serviceGroupVM.CreatedDate;
            serviceGroup.Description = serviceGroupVM.Description;
            serviceGroup.DisplayOrder = serviceGroupVM.DisplayOrder;
            serviceGroup.Image = serviceGroupVM.Image;
            serviceGroup.MetaDescription = serviceGroupVM.MetaDescription;
            serviceGroup.MetaKeyWord = serviceGroupVM.MetaKeyWord;
            serviceGroup.Name = serviceGroupVM.Name;
            serviceGroup.MainServiceGroupId = serviceGroupVM.MainServiceGroupId;

            serviceGroup.Status = serviceGroupVM.Status;
            serviceGroup.UpdatedBy = serviceGroupVM.UpdatedBy;
            serviceGroup.UpdatedDate = serviceGroupVM.UpdatedDate;
        }

        public static void UpdateMainServiceGroup(this MainServiceGroup mainServiceGroup, MainServiceGroupViewModel mainServiceGroupVM)
        {
            mainServiceGroup.Id = mainServiceGroupVM.Id;

            mainServiceGroup.CreatedBy = mainServiceGroupVM.CreatedBy;
            mainServiceGroup.CreatedDate = mainServiceGroupVM.CreatedDate;
            mainServiceGroup.MetaDescription = mainServiceGroupVM.MetaDescription;
            mainServiceGroup.MetaKeyWord = mainServiceGroupVM.MetaKeyWord;
            mainServiceGroup.Name = mainServiceGroupVM.Name;

            mainServiceGroup.Status = mainServiceGroupVM.Status;
            mainServiceGroup.UpdatedBy = mainServiceGroupVM.UpdatedBy;
            mainServiceGroup.UpdatedDate = mainServiceGroupVM.UpdatedDate;
        }

        public static void UpdateService(this PostOffice.Model.Models.Service service, ServiceViewModel serviceVM)
        {
            service.ID = serviceVM.ID;
            service.Alias = serviceVM.Alias;
            service.CreatedBy = serviceVM.CreatedBy;
            service.CreatedDate = serviceVM.CreatedDate;
            service.Description = serviceVM.Description;
            service.BuyIn = serviceVM.BuyIn;
            service.SoldOut = serviceVM.SoldOut;
            service.VAT = serviceVM.VAT;
            service.CreatedBy = serviceVM.CreatedBy;
            service.CreatedDate = serviceVM.CreatedDate;
            service.Description = serviceVM.Description;
            service.GroupID = serviceVM.GroupID;
            service.PayMethodID = serviceVM.PayMethodID;
            service.MetaDescription = serviceVM.MetaDescription;
            service.MetaKeyWord = serviceVM.MetaKeyWord;
            service.Name = serviceVM.Name;
            service.Status = serviceVM.Status;
            service.UpdatedBy = serviceVM.UpdatedBy;
            service.UpdatedDate = serviceVM.UpdatedDate;
        }

        public static void UpdatePropertyService(this PropertyService prop, PropertyServiceViewModel vm)
        {
            prop.CreatedBy = vm.CreatedBy;
            prop.CreatedDate = vm.CreatedDate;
            prop.ID = vm.ID;
            prop.MetaDescription = vm.MetaDescription;
            prop.MetaKeyWord = vm.MetaKeyWord;
            prop.Name = vm.Name;
            prop.Percent = vm.Percent;
            prop.ServiceID = vm.ServiceID;
            prop.UpdatedBy = vm.UpdatedBy;
            prop.Status = vm.Status;
            prop.UpdatedDate = vm.UpdatedDate;
        }


        public static void UpdatePaymentMethod(this PaymentMethod payment, PaymentMethodViewModel paymentVM)
        {
            payment.CreatedBy = paymentVM.CreatedBy;
            payment.CreatedDate = paymentVM.CreatedDate;
            payment.Description = paymentVM.Description;
            payment.ID = paymentVM.ID;
            payment.MetaDescription = paymentVM.MetaDescription;
            payment.MetaKeyWord = paymentVM.MetaKeyWord;
            payment.Name = paymentVM.Name;
            payment.Status = paymentVM.Status;
            payment.UpdatedBy = paymentVM.UpdatedBy;
            payment.UpdatedDate = paymentVM.UpdatedDate;
            payment.Value = paymentVM.Value;
        }

    }
}