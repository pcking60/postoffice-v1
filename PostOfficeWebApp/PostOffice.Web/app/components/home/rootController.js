(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state',  '$scope', 'authService'];

    function rootController($state, $scope, authService) {
        $scope.logOut = function () {
            authService.logOut();
            $state.go('login');
        }
        $scope.authentication = authService.authentication;

        //authenticationService.validateRequest();
    }
})(angular.module('postoffice'));