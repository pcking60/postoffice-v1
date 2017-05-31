(function (app) {
    app.controller('transactionAddController', transactionAddController);
    transactionAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams', '$ngBootbox', '$timeout'];
    function transactionAddController($scope, apiService, notificationService, $state, commonService, $stateParams, $ngBootbox, $timeout) {
        $scope.transaction = {
            Status: true,
            Service: null,
            Quantity: null,
            TransactionDate: null,
            Properties: [],
            TransactionDetails: []
        }

        $scope.AddTransaction = AddTransaction;
        function AddTransaction() {
            $scope.transaction.ServiceId = $scope.transaction.Service.ID;           
            $scope.transaction.TransactionDate = $("#datetimepicker1").find("input").val();
            
            const ACCEPTABLE_OFFSET = 172800 * 1000;
            if ((new Date().getTime() - new Date($scope.transaction.TransactionDate).getTime()) > ACCEPTABLE_OFFSET)
            {
                notificationService.displayError('Ngày giao dịch đã chậm quá 2 ngày');
            }
            else
            {
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
                        notificationService.displaySuccess('Giao dịch thành công');
                        $timeout(function () {
                            $ngBootbox.confirm('Bạn có muốn tiếp tục nhập?').then(function () {
                            }
                            , function () {
                                $state.go('transactions', {}, { reload: true });
                                //$state.reload();                        
                            });
                        }, 500);

                    }, function (error) {
                        notificationService.displayError('Giao dịch thất bại');
                    });
            }            
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
        var dateTo = new Date();

        
        loadServiceDetail();
        getPropertyServices();
    }
})(angular.module('postoffice.transactions'));