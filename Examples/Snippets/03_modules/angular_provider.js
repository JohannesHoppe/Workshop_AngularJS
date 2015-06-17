angular.module('exampleApp', [])
 
    .service('helloWorldService', function() {
        this.sayHello = function() {
            return "Hello World!";
        };
    })
 
    .factory('helloWorldFactory', function() {
        return {
            sayHello: function() {
                return "Hello World!";
            }
        };
    })
    
    .provider('helloWorldProvider', function () {

        this.$get = function() {
            return {
                sayHello: function() {
                    return "Hello World!";
                }
            }
        };
    })

    .controller('exampleController', function ($scope, helloWorldService, helloWorldFactory, helloWorldProvider) {

        $scope.helloArray = [
            helloWorldService.sayHello(),
            helloWorldFactory.sayHello(),
            helloWorldProvider.sayHello()
        ];
    });