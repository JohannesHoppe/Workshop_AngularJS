![Logo](Images/DeveloperWeek2015_klein.jpg)

# Einstieg in AngularJS - Lektion 2: Routing

## Routing mit angular-route


Es fehlt noch ein Prinzip, welches für eine SPA unerlässlich ist: **Client-side Routing**.  
"Routing" bedeutet, dass die Anwendung zwischen Ansichten wechseln kann und dabei die Browser-History aktualisiert. Es wird dadurch möglich, den "Zurück"- und "Vor"-Button des Browser wie gewohnt zu verwenden. Ebenso sollte das Routing sicherstellen, dass man zu eine beliebigen Ansicht springen kann, indem man die entsprechende URL im Browser aufruft. Ist das Routing gut implementiert, ist für den Anwender nicht mehr ersichtlich, ob es sich um eine "klassische" Anwendung mit mehreren HTML-Seiten oder eine SPA handelt (wobei natürlich die Vorteile von Single-Page, wie z.B. schnelle Ladezeiten erhalten bleiben sollten).

Angular setzt hier auf den $routeProvider, welcher die History überwacht und bei Bedarf ein Template lädt und den passenden Controller aufruft. Die Verwendung ist schnell ersichtlich. Man definiert eine `ngView` Direktive[1]

```html
<body ng-app="exampleApp" class="example">
    <div ng-view></div>
</body>
```

konfiguriert entsprechend den `$routeProvider`:


```javascript

angular.module('app', ['ngRoute'])

    .config(function($routeProvider) {

        $routeProvider.when('/list', {
            templateUrl: 'templates/list.html',
            controller: 'listController'
        });

        $routeProvider.when('/detail/:id', {
            templateUrl: 'templates/detail.html',
            controller: 'detailController'
        });

        $routeProvider.otherwise({ redirectTo: '/list' });
    })

    .controller('listController', function ($scope) {
        
        /* logik */
    })

    .controller('detailController', function ($scope, $routeParams) {
        
        /* logik */
        console.log($routeParams.id);
    });
```

## Direktiven

Direktiven sind Marker im HTML, welche dem HTML compiler (`$compile`) von AngularJS Instruktionen geben. Es wird dadurch eine sehr deklarative Beschreibung der Applikation möglich.





## Aufgabe

1. Erstelle eine Detailansicht für den Kundenmanager mittels Routing.
2. Vereinfache deinen Code mittels einer Direktive!

<hr>

[1] ngView: https://docs.angularjs.org/api/ngRoute/directive/ngView

<hr>

&copy; 2015, Johannes Hoppe