(function (app) {  
    app.controller('transactionAddController', transactionAddController);
    transactionAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams'];
    function transactionAddController($scope, apiService, notificationService, $state, commonService, $stateParams) {
        $scope.transaction = {
            Status: true,
            Service: null,
            Quantity: null,
            TransactionDate : null,
            Status: null,
            transactionDetails: []
        }
       
        $scope.AddTransaction = AddTransaction;
        function AddTransaction() {
            $scope.transaction.ServiceId = $scope.transaction.Service.ID;
            apiService.post('/api/transactions/create', $scope.transaction,
                function (result) {
                    notificationService.displaySuccess('Giao dịch thành công!');
                    $state.go('transactions');
                }, function (error) {
                    notificationService.displayError('Giao dịch thất bại');
                });
        }

        function getPropertyServices() {
            apiService.get('/api/property_services/getbyserviceid/' + $stateParams.id, null, function (result) {
                $scope.transaction.Properties = result.data;
                result.data.forEach(function (item, index) {
                    $scope.transaction.transactionDetails.push({
                        Money: null,
                        PropertyServiceId: item.PropertyServiceId,
                        PropertyService: item,
                        TransactionId: null
                    });
                });
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