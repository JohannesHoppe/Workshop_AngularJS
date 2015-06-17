define(['require', 'angular'], function (require, angular) {

    angular.module('exampleApp', [])

        .directive('stickyNote', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    title: '@',
                    message: '@',
                },
                templateUrl: 'angular.tmpl.html'
            }
        })

        .controller('exampleController', function ($scope) {

            $scope.model = {
                title: "Remember",
                message: "the milk"
            }

        });

    require(['domReady!'], function (domReady) {
        angular.bootstrap(domReady, ['exampleApp']);
    });

});
