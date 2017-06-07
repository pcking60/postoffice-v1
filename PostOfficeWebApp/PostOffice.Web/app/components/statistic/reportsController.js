﻿(function (app) {
    app.controller('reportsController', reportsController);

    reportsController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', 'authService'];

    function reportsController($scope, apiService, notificationService, $filter, authService) {       
        $scope.report = {
            units: [],
            functionId: 0,
            unitId: 0
        };
        
        $scope.report.date = { startDate: null, endDate: null };
        $scope.functions =
            [
                { Id: 1, Name: 'Báo cáo Bảng kê thu tiền tại đơn vị' },
                { Id: 2, Name: 'Báo cáo tiền lương tại đơn vị - test' },
                { Id: 3, Name: 'Báo cáo doanh thu tại đơn vị - test' },

            ]
        $scope.getUnit = getUnit;     
        function getUnit() {
            apiService.get('/api/district/getallparents',
                null,
                function (response) {
                    $scope.report.units = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách đơn vị.');
                });
        }
       
        $scope.Report = Report;
        function Report() {
            $scope.loading = false;
            var fromDate = $scope.report.date.startDate.format('MM/DD/YYYY');
            var toDate = $scope.report.date.endDate.format('MM/DD/YYYY');
            var config = {
                params: {
                    //mm/dd/yyyy
                    fromDate: fromDate,
                    toDate: toDate,
                    functionId: $scope.report.functionId || 0,
                    unitId: $scope.report.unitId || 0                    
                }
            }
            apiService.get('api/statistic/reportFunction1', config,
                function (response) {
                    $scope.loading = true;
                    if (response.status = 200) {
                        window.location.href = response.data.Message;
                    }

                },
                function (response) {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                    }
                    else {
                        notificationService.displayError('Không thể tải dữ liệu');
                    }
                }
            )
        }

        getUnit();

    }

})(angular.module('postoffice.statistics'));