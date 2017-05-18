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
            Properties: [],
            transactionDetails: []
        }
       
        $scope.AddTransaction = AddTransaction;
        function AddTransaction() {
            $scope.transaction.ServiceId = $scope.transaction.Service.ID;
            $scope.transaction.TransactionDate = new Date($scope.transaction.TransactionDate);
            apiService.post('/api/transactions/create', $scope.transaction,
                function (result) {
                    console.log('Giao dịch thành công!');                    
                }, function (error) {
                    notificationService.displayError('Giao dịch thất bại');
                });
            apiService.post('/api/transactiondetails/create', $scope.transaction.transactionDetails,
               function (result) {
                   //$scope.transaction.Properties.forEach(function (item, index) {
                   //    $scope.transaction.transactionDetails.push({
                   //        Money: item.Money,
                   //        PropertyServiceId: item.ID,
                   //        Status: true,
                   //        TransactionId: null
                   //    });
                   //});
                   //notificationService.displaySuccess('Giao dịch thành công!');
                   $state.go('transactions');
               }, function (error) {
                   notificationService.displayError('Giao dịch thất bại');
               });

        }

        function getPropertyServices() {
            apiService.get('/api/property_services/getbyserviceid/' + $stateParams.id, null, function (result) {
                $scope.transaction.Properties = result.data;
               
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