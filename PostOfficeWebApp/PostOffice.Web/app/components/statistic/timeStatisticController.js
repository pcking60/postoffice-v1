(function (app) {
    app.controller('timeStatisticController', timeStatisticController);

    timeStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', 'authService'];

    function timeStatisticController($scope, apiService, notificationService, $filter, authService) {
        $scope.statisticResult = [];
        $scope.test = {
            users: []           
        };
        $scope.test.ServiceId = 0;
        $scope.test.UserId = '';
        $scope.test.date = { startDate: null, endDate: null };
        $scope.getListUser = getListUser;
        $scope.getService = getService;

        $scope.isManager = authService.haveRole("Manager");
        console.log($scope.isManager);
        function getListUser() {
            apiService.get('/api/applicationUser/getuserbypoid',
                null,
                function (response) {
                    $scope.test.users = response.data;
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
        //check role 
        $scope.isManager = authService.haveRole('Manager');
        $scope.isAdmin = authService.haveRole('Administrator');
        $scope.chartdata = [];
        $scope.TimeStatistic = TimeStatistic;
        function TimeStatistic() {
            var fromDate = $scope.test.date.startDate.format('MM-DD-YYYY');
            var toDate = $scope.test.date.endDate.format('MM-DD-YYYY');
            var config = {
                params: {
                    //mm/dd/yyyy
                    fromDate: fromDate,
                    toDate: toDate,
                    serviceId: $scope.test.ServiceId || 0,
                    userId: $scope.test.UserId || ''
                }
            }
            apiService.get('api/transactions/stattistic', config,
                function (response) {
                    $scope.statisticResult = response.data;
                    $scope.result = true;
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