﻿'use strict';
angular.module('postoffice.common')
    .service('authService', ['$http', '$q', 'localStorageService', 'notificationService', function ($http, $q, localStorageService, notificationService) {
               
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: "",
            fullName: "",
            roles: [],
            isAdmin: false,
            isManager: false
        };        


        var _haveRole = function haveRole(roleName) {
            var isValid = false;
            var Roles = _authentication.roles;
            angular.forEach(Roles, function (role) {
                if (role.Name == roleName) {
                    isValid = true;
                }
            })
            return isValid;
        }

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            var deferred = $q.defer();

            $http.post('/oauth/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

                localStorageService.set('authorizationData', {
                    token: response.data.access_token,
                    userName: loginData.userName,
                    fullName: response.data.fullName,
                    roles : JSON.parse(response.data.permissions)                    
                });
                _authentication.roles = JSON.parse(response.data.permissions);
                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;                
                _authentication.fullName = response.data.fullName;
                deferred.resolve(response);

            }).catch(function (err, status) {
                _logOut();
                notificationService.displayError("Đăng nhập không đúng.");
                deferred.reject(err);
            });

            return deferred.promise;

        };
        var _logOut = function () {

            localStorageService.remove('authorizationData');           
            _authentication.isAuth = false;
            _authentication.userName =
            _authentication.fullName = "";
            _authentication.roles = [];
            _authentication.isAdmin= false;
            _authentication.isManager = false;
           
        };
        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
                _authentication.fullName = authData.fullName;
                _authentication.roles = authData.roles;                
            }

        }        

        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.haveRole = _haveRole;

        return authServiceFactory;
}]);
