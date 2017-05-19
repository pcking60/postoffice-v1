﻿(function (app) {
    app.controller('transactionEditController', transactionEditController);
    transactionEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService', '$timeout'];
    function transactionEditController($scope, apiService, notificationService, $state, $stateParams, commonService, $timeout) {
        $scope.transaction = {
            Status: true,
            TransactionDetails: [],
            Service: null
        }
        $scope.EditTransaction = EditTransaction;
        //$scope.changeSomething = changeSomething;
        //function changeSomething()
        //{
        //    $scope.mainServiceGroup.Name = commonService.toTitleCase($scope.mainServiceGroup.Name);
        //}
        
        function loadTransactionDetail()
        {
            apiService.get('/api/transactions/getbyid/' + $stateParams.id, null, function (result) {
               
                $timeout(function () {

                }, 200);
                $scope.transaction = result.data;              
                
                apiService.get('/api/transactiondetails/getbytransactionid/' + $scope.transaction.ID, null, function (result) {
                    $timeout(function () {
                        $scope.transaction.TransactionDetails = result.data;
                    }, 200);                    

                }, function (error) {
                    notificationService.displayError(error.data)
                });
            }, function (error) {
                notificationService.displayError(error.data)
            });
        }

        //function getTransactionDetail() {
        //    apiService.get('/api/transactiondetails/getbytransactionid/' + $stateParams.id, null, function (result) {
        //        $scope.transaction.TransactionDetails = result.data;

        //    }, function (error) {
        //        notificationService.displayError(error.data)
        //    });
        //}
        function EditTransaction() {
            $scope.transaction.TransactionDate = $("#datetimepicker1").find("input").val();
            apiService.put('/api/transactions/update', $scope.transaction,
                function (result) {
                    notificationService.displaySuccess('Cập nhật thành công');
                    $state.go('transactions', {}, {reload: true});
                }, function (error) {
                    notificationService.displayError('Cập nhật thất bại');
                });
        }
        //getTransactionDetail();
        loadTransactionDetail();
        
    }
})(angular.module('postoffice.transactions'));