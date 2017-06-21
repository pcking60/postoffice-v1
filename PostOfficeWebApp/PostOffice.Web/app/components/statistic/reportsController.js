(function (app) {
    app.controller('reportsController', reportsController);

    reportsController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', 'authService', '$stateParams'];

    function reportsController($scope, apiService, notificationService, $filter, authService, $stateParams) {
        $scope.report = {
            districts: [],
            units: [],
            functionId: 0,
            unitId: 0,
            districtId: 0
        };
        $stateParams.id = 0;
        $scope.report.date = { startDate: null, endDate: null };
        $scope.functions =
            [
                { Id: 1, Name: 'Bảng kê thu tiền tại bưu cục' },
                { Id: 2, Name: 'Báo cáo tiền lương tại đơn vị - test' },
                { Id: 3, Name: 'Báo cáo doanh thu tại đơn vị - test' },

            ]
        $scope.getUnit = getUnit;
        $scope.getDistrict = getDistrict;
        $scope.Reset = Reset;
        function getDistrict(){
            apiService.get('/api/district/getallparents',
                null,
                function (response) {
                    $scope.report.districts = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách huyện.');
                }
            );
        }
        function getUnit() {
            apiService.get('/api/po/getbydistrictid/ ' + $stateParams.id,
                null,
                function (response) {
                    $scope.report.units = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách đơn vị.');
                }
            );
        }

        function Reset() {
            $scope.report.districtId = 0;
            $scope.report.unitId = 0;
        }

        $scope.onSelectCallback = function (item, model) {
            $stateParams.id = item.ID;
            $scope.report.unitId = 0;
            getUnit();
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
                    districtId: $scope.report.districtId || 0,
                    functionId: $scope.report.functionId || 0,
                    unitId: $scope.report.unitId || 0                    
                }
            }
            apiService.get('api/statistic/rp1', config,
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

        getDistrict();        
    }

})(angular.module('postoffice.statistics'));