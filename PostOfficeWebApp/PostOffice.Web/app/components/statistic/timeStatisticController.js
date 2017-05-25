(function (app) {
    app.controller('timeStatisticController', timeStatisticController);

    timeStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function timeStatisticController($scope, apiService, notificationService, $filter) {
        $scope.statisticResult = [];
        $scope.test = [];
        $scope.test.date = { startDate: null, endDate: null };
        $scope.getListUser = getListUser;
        $scope.getService = getService;

        function getListUser() {
            apiService.get('/api/applicationUser/getuserbypoid',
                null,
                function (response) {
                    $scope.users = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhân viên.');
                });
        }

        function getService() {
            apiService.get('/api/service/getallparents',
                null,
                function (response) {
                    $scope.services = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách dịch vụ.');
                });
        }

        $scope.chartdata = [];
        $scope.TimeStatistic = TimeStatistic;
        function TimeStatistic() {
            var fromDate = $scope.test.date.startDate.format('MM-DD-YYYY');
            var toDate = $scope.test.date.endDate.format('MM-DD-YYYY');
            var userId = $scope.test.UserID;
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: fromDate,
                    toDate: toDate
                }
            }
            apiService.get('api/transactions/stattistic?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null,
                function (response) {
                    $scope.statisticResult = response.data;
                    console.log(response.data);                    
                },
                function (response) {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                    }
                    else {
                        notificationService.displayError('Không thể tải dữ liệu');
                    }
                });
        }
        
        getListUser();
        getService();

    }

})(angular.module('postoffice.statistics'));