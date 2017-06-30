/// <reference path="/Assets/admin/libs/angular/angular.js" />
'use strict';
angular.module('postoffice.tkbd', ['postoffice.common'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('tkbds', {
                url: "/tkbd",
                templateUrl: "/app/components/tkbd/tkbdListView.html",
                parent: 'base',
                controller: "tkbdListController"
            });
        }])



