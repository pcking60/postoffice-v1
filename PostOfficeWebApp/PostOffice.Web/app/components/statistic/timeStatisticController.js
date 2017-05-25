(function (app) {
    app.controller('timeStatisticController', timeStatisticController);

    timeStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function timeStatisticController($scope, apiService, notificationService, $filter) {
        $scope.statisticResult = [];
        $scope.test = {};
        $scope.test.date = { startDate: null, endDate: null };

        $scope.chartdata = [];
        $scope.TimeStatistic = TimeStatistic;
        function TimeStatistic() {
            var fromDate = $scope.test.date.startDate.format('MM-DD-YYYY');
            var toDate = $scope.test.date.endDate.format('MM-DD-YYYY');

            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: fromDate,
                    toDate: toDate
                }
            }
            apiService.get('api/transactions/getallbytime?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null,
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
        //TimeStatistic();
    }

})(angular.module('postoffice.statistics'));