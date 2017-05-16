/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.transaction_details', ['postoffice.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/transaction_details');

        $stateProvider
            
            .state('transaction_details', {
                url: "/transaction_details",
                parent: 'base',
                templateUrl: "/app/components/transaction_details/transactionsListView.html",
                controller: "transactionsListController"
            }).state('add_transaction_detail', {
                url: "/add_transaction_detail",
                parent: 'base',
                templateUrl: "/app/components/transaction_details/transactionDetailAddView.html",
                controller: "transactionDetailAddController"
            }).state('edit_transaction_detail', {
                url: "/edit_transaction_detail/:id",
                parent: 'base',
                templateUrl: "/app/components/transaction_details/transactionDetailEditView.html",
                controller: "transactionDetailEditController"
            });
    };
})();