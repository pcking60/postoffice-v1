(function (app) {
    app.controller('serviceListController', serviceListController);
    serviceListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];
    function serviceListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.services = [];
        $scope.getServices = getServices;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteService = deleteService;
        $scope.loading = true;
        function deleteService(id) {
            $ngBootbox.confirm('Bạn có chắc xóa không?')
                .then(
                    function () {
                        config =
                            {
                                params: {
                                    id: id
                                }
                            }

                apiService.del(
                    '/api/service/delete',
                    config,
                    function () {
                        notificationService.displaySuccess('Xóa dữ liệu thành công');
                        search();
                    },
                    function () {
                        notificationService.displaySuccess('Xóa dữ liệu thất bại');
                    }

                );

            }, function () {
                console.log('Command was cancel!');
            });
        }
        function search() {
            getServices();
        }
        function getServices(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/service/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có dữ liệu");
                }
                $scope.loading = false;
                $scope.services = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            },
            function () {
                console.log('Load service failed');
                $scope.loading = false;
            });
        } $scope.getServices();
    }
})(angular.module('postoffice.services'));
