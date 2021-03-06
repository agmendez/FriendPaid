﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Exceptions;
using FluentValidation.Results;
using Services;
using Web.Validators;
using Web.ViewModels;
using Web.ViewModelsBuilders;

namespace Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;

        private readonly IUserService userService;
        
        public GroupController(IGroupService groupService, IUserService userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        public ActionResult IndexGroups(string username)
        {
            var newViewModel = new GroupsViewModel();

            newViewModel.username = username;

            var currentUser = userService.GetByUsername(username);

            var groupsNamesJoined = new List<string>();

            currentUser.groups.ToList().ForEach(group => groupsNamesJoined.Add(group.name));

            newViewModel.groups = groupService.GetAll().Where(group => group.administrator.username.Equals(username) || groupsNamesJoined.Contains(group.name) ).ToList();
            
            return View(newViewModel);
        }

        public ActionResult IndexCreateGroup(string username)
        {
            var newViewModel = new CreateGroupViewModel();
            newViewModel.username = username;
            return View(newViewModel);
        }

        public ActionResult Create(CreateGroupViewModel viewModel)
        {
            var createGroupValidator = new CreateGroupValidator(groupService);

            var validationResult = createGroupValidator.Validate(viewModel);

            var user = userService.GetByUsername(viewModel.username);

            if (validationResult.IsValid){
                userService.CreateGroup(user,viewModel.groupName);
                viewModel.message = "Se ha creado satisfactoriamente el grupo.";
            }else{

                var stringErrors = new List<string>();
                validationResult.Errors.ToList().ForEach(oneError => stringErrors.Add(oneError.ErrorMessage));
                viewModel.errors = stringErrors;

            }
            return View("IndexCreateGroup", viewModel);
        }

        public ActionResult Join(string username,string groupname)
        {
            var viewModel = new GroupsViewModel() {username = username};

            var currentUser = userService.GetByUsername(username);

            var currentGroup = groupService.GetByName(groupname);

            userService.JoinGroup(currentUser, currentGroup);

            var groupsNamesJoined = new List<string>();

            currentUser.groups.ToList().ForEach(group => groupsNamesJoined.Add(group.name));

            viewModel.groups = groupService.GetAll().Where(group => group.administrator.username.Equals(username) || groupsNamesJoined.Contains(group.name)).ToList();

            viewModel.message = "Te has unido satisfactoriamente al grupo.";

            return View("IndexGroups",viewModel);
        }
        public ActionResult Leave(string username,string groupname)
        {

            var viewModel = new GroupsViewModel() {username = username};

            var currentUser = userService.GetByUsername(username);

            var currentGroup = groupService.GetByName(groupname);

            //TODO: El administrador podria abandonar el grupo si solo si es el unico miembro, entonces deberia borrarse el grupo.
            try{
                userService.LeaveGroup(currentUser, currentGroup);
                viewModel.message = "Has abandonado el grupo satisfactoriamente.";
            }catch(AdministratorCantLeaveGroupException){

                viewModel.errors= new List<string>(){"El administrador no puede abandonar el grupo."};
            }

            var groupsNamesJoined = new List<string>();

            currentUser.groups.ToList().ForEach(group => groupsNamesJoined.Add(group.name));

            viewModel.groups = groupService.GetAll().Where(group => group.administrator.username.Equals(username) || groupsNamesJoined.Contains(group.name) ).ToList();

            return View("IndexGroups", viewModel);
        }

    }
}
