(function (app) {  
    app.controller('transactionAddController', transactionAddController);
    transactionAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams'];
    function transactionAddController($scope, apiService, notificationService, $state, commonService, $stateParams) {
        $scope.transaction = {
            Status: true,
            Service: null,
            Properties: null
        }
        $scope.transactionDetail = {
            TransactionId: null,
            PropertyServiceId: null,
            Money: null
        }
       
        $scope.AddTransaction = AddTransaction;
        function AddTransaction() {
            $scope.transaction.ServiceId = $scope.service.ID;
            apiService.post('/api/transaction/create', $scope.transaction,
                function (result) {
                    notificationService.displaySuccess('Giao dịch thành công!');
                    $state.go('transactions');
                }, function (error) {
                    notificationService.displayError('Giao dịch thất bại');
                });
        }

        function getPropertyServices() {
            apiService.get('/api/property_services/getbyserviceid/' + $stateParams.id, null, function (result) {
                $scope.transaction.Properties = result.data
            }, function (error) {
                notificationService.displayError(error.data)
            });
        }

        function loadServiceDetail() {
            apiService.get('/api/service/getbyid/' + $stateParams.id, null, function (result) {
                $scope.transaction.Service = result.data
            }, function (error) {
                notificationService.displayError(error.data)
            });
        }
        loadServiceDetail();
        getPropertyServices();
    }
})(angular.module('postoffice.transactions'));