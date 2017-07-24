(function (app) {
    'use strict';

    app.controller('userProfileViewController', userProfileViewController);

    userProfileViewController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function userProfileViewController($scope, apiService, notificationService, $ngBootbox) {
        var vm = this;
        vm.tab = 1;
        vm.selectTab = function (setTab) {
            vm.tab = setTab;
        };
        vm.isTab = function (checkTab) {
            return vm.tab === checkTab;
        };
    }
})(angular.module('postoffice.application_users'));