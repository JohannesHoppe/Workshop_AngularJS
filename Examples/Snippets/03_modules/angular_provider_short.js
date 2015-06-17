angular.module('exampleApp', [])
 
    .service('helloWorldService', function() {
        this.sayHello = function() {
            return "Hello World!";
        };
    })

    .controller('exampleController', function ($scope, helloWorldService) {
        $scope.hello = helloWorldService.sayHello();
    });