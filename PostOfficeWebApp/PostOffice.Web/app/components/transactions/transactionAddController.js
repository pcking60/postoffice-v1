(function (app) {
    app.controller('transactionAddController', transactionAddController);
    transactionAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams'];
    function transactionAddController($scope, apiService, notificationService, $state, commonService, $stateParams) {
        $scope.transaction = {
            Status: true,
            Service: null,
            Quantity: null,
            TransactionDate: null,
            Status: null,
            Properties: [],
            TransactionDetails: []
        }

        $scope.AddTransaction = AddTransaction;
        function AddTransaction() {
            $scope.transaction.ServiceId = $scope.transaction.Service.ID;
            var d = new Date()
            var n = d.getTimezoneOffset();
            console.log(n);
            $scope.transaction.TransactionDate = $("#datetimepicker1").find("input").val();
            //var date = $scope.transaction.TransactionDate;
            //$scope.transaction.TransactionDate = new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(),  date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
            $scope.transaction.Properties.forEach(function (item, index) {
                $scope.transaction.TransactionDetails.push({
                    Money: item.Money,
                    PropertyServiceId: item.ID,
                    Status: true,
                    TransactionId: -1
                });
            });

            // trong cái này mình đã chứa đủ dữ liệu rồi 
            apiService.post('/api/transactions/create', $scope.transaction,
                function (result) {
                    $state.go('transactions');
                    console.log('Giao dịch thành công!');
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