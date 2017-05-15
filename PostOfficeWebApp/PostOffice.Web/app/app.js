﻿/// <reference path="/Assets/admin/libs/angular/angular.js" />

angular.module('postoffice',
            [
                'postoffice.application_groups',
                'postoffice.application_roles',
                'postoffice.application_users',
                'postoffice.districts',
                'postoffice.services',
                'postoffice.service_groups',
                'postoffice.main_service_groups',
                'postoffice.pos',
                'angular-loading-bar',
                'LocalStorageModule',                
                'ngCookies',
                'ngBootstrap',
                'postoffice.common'
            ])

    .config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        //$locationProvider.html5Mode({
        //    enabled: true, 
        //    requireBase: false,
        //    hashPrefix: ''
        //})

        $stateProvider
            .state('base', {
                url: '',
                templateUrl: '/app/shared/views/baseView.html',
                abstract: true
            })
            .state('userbase', {
                url: '',
                templateUrl: '/app/shared/views/userBaseView.html',
                abstract: true
            })
            .state('login', {
                url: "/login",
                templateUrl: "/app/components/login/loginView.html",
                controller: "loginController"
            })
            .state('user_dashboard', {
                url: "/user_dashboard",
                parent: 'userbase',
                templateUrl: "/app/components/user_dashboard/userDashboardView.html",
                controller: "userDashboardController"
            })
            .state('home', {
                url: "/admin",
                parent: 'base',
                templateUrl: "/app/components/home/homeView.html",
                controller: "homeController"
            });
        $urlRouterProvider.otherwise('/login');
    })

    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    })

    .run(['authService', function (authService) {
        authService.fillAuthData();
    }])

    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeBar = true;
        cfpLoadingBarProvider.includeSpinner = true;
    }]);

  

