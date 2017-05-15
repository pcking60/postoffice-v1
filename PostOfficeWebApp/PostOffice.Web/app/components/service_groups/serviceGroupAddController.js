(function (app) {
   

    
    app.controller('serviceGroupAddController', serviceGroupAddController);
    serviceGroupAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService'];
    function serviceGroupAddController($scope, apiService, notificationService, $state, commonService) {
        $scope.serviceGroup = {
            Status: true
        }
        $scope.changeSomething = changeSomething;
        function changeSomething() {
            $scope.serviceGroup.Alias = commonService.getSeoTitle($scope.serviceGroup.Name);
            $scope.serviceGroup.Name = commonService.toTitleCase($scope.serviceGroup.Name);
        }
        $scope.AddServiceGroup = AddServiceGroup;
        function AddServiceGroup() {
            apiService.post('/api/servicegroup/create', $scope.serviceGroup,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công!');
                    $state.go('service_groups');
                }, function (error) {
                    notificationService.displayError('Thêm mới thất bại');
            });
        }
        function loadparentGroup() {
            apiService.get('/api/servicegroup/getallparents', null, function (result) {
                $scope.parentGroups = result.data;
            }, function () {
                console.log('Can not load Parent Group');
            }); 
        }
        loadparentGroup();
    }
})(angular.module('postoffice.service_groups'));