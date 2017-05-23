(function (app) {
    app.controller('transactionsListController', transactionsListController);
    transactionsListController.$inject = ['$scope', 'apiService', 'notificationService',
            '$ngBootbox', '$filter', '$state', 'authService'];
    function transactionsListController($scope, apiService, notificationService, $ngBootbox, $filter, $state, authService) {
             
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.transactions = [];
        $scope.getTransactions = getTransactions;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteTransaction = deleteTransaction;

        //test gettime()
        $scope.currentDate = new Date();     
        //$scope.selectAll = selectAll;
        //$scope.deleteMulti = deleteMulti;
        $scope.loading = true;
        
        //function deleteMulti() {
        //    var listId = [];
        //    $.each($scope.selected, function (i, item) {
        //        listId.push(item.ID);
        //    });
        //    var config = {
        //        params: {
        //            checkedTransactions: JSON.stringify(listId)
        //        }
        //    }
        //    $ngBootbox.confirm('Bạn có chắc xóa không?').then(
        //        function () {
        //            apiService.del('/api/mainservicegroup/deletemulti', config, function (result) {
        //                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
        //                search();
        //            }, function (error) {
        //                notificationService.displayError('Xóa không thành công');
        //            });
        //        }, function () {
        //            console.log('Command was cancel');
        //        });
           
        //}
        //$scope.isAll = false;
        //function selectAll() {
        //    if ($scope.isAll === false) {
        //        angular.forEach($scope.transactions, function (item) {
        //            item.checked = true;
        //        });
        //        $scope.isAll = true;
        //    } else {
        //        angular.forEach($scope.transactions, function (item) {
        //            item.checked = false;
        //        });
        //        $scope.isAll = false;
        //    }
        //}

        //$scope.$watch("transactions", function (n, o) {
        //    var checked = $filter("filter")(n, { checked: true });
        //    if (checked.length) {
        //        $scope.selected = checked;
        //        $('#btnDelete').removeAttr('disabled');
        //    } else {
        //        $('#btnDelete').attr('disabled', 'disabled');
        //    }
        //}, true);

        //function deleteTransaction(id) {
        //    $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
        //        var config = {
        //            params: {
        //                id: id
        //            }
        //        }
        //        apiService.del('/api/transactions/delete', config, function () {
        //            notificationService.displaySuccess('Xóa mẫu tin thành công');
        //            search();
        //        }, function () {
        //            notificationService.displayError('Xóa mẫu tin thất bại!');
        //        });
        //    }, function () {
        //        console.log('Command was cancel');
        //    });
        //}

        
        function deleteTransaction(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/transactions/delete', config,
                    function (result) {
                        notificationService.displaySuccess('Giao dịch đã được khóa');
                        $state.reload();
                        //$state.go('transactions');
                    }, function (error) {
                        notificationService.displayError('Cập nhật thất bại');
                    });
                }, function () {
                console.log('Command was cancel');
            });
         }

        function search() {
            getTransactions();
            $state.go('userbase', {}, { reload: true });
        }
        function getTransactions(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword : $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/transactions/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Chưa có dữ liệu");
                    
                }               
                
                $scope.transactions = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                
                $scope.loading = false;
            },
            function () {
                $scope.loading = false;
                console.log('Load transactions failed');
            });

        }

        $scope.authentication = authService.authentication;
        var userName = $scope.authentication.userName;

        function getUserInfo() {
            apiService.get('/api/applicationUser/getuserinfo/' + userName, null, function (result) {
                $scope.userInfo = result.data;
            }, function () {
                console.log('Can not load user info!');
            });
        }

        const ACCEPTABLE_OFFSET = 172800*1000;

        $scope.editEnabled = function(transaction)
        {
            return (new Date().getTime() - (new Date(transaction.TransactionDate)).getTime()) > ACCEPTABLE_OFFSET;
        }

        getUserInfo();

        getTransactions();
        
    }

    
})(angular.module('postoffice.transactions'));