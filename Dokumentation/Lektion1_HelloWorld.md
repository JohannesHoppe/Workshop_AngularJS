![Logo](Images/DeveloperWeek2015_klein.jpg)

# Einstieg in AngularJS - Lektion 1: Hello World mit require.js

## Hello World

AngularJS ist ein MVC Framework. Mittels der Directive `ngApp` wird hier das Modul "exampleApp"  mit dem darin enthaltenen Controller "exampleController" ausgeführt.  Hinter dem Befehl versteckt sich ein mehrstufiger Prozess, den AngularJS schlicht "Bootstrapping" nennt. Dies geschieht, sobald das HTML-Dokument komplett fertig geladen wurde (`DOMContentLoaded` Event).

##### Listing 1a -- HelloWorld.cshtml
~~~~~
<!DOCTYPE html>
<html>
<body ng-app="exampleApp">

    <div ng-controller="exampleController">
        <h1 ng-bind="model.text"></h1>
    </div>

    <script src="Scripts/angular.js"></script>
    <script src="Scripts/helloWorld.js"></script>
</body>
</html>
~~~~~

##### Listing 1b -- Die Datei helloWorld.js mit einem Angular-Modul
~~~~~
angular.module('exampleApp', [])
    .controller('exampleController', function($scope) {

        $scope.model = {
            text: 'Hello World'
        }
    });
~~~~~

## Require.js

Um JavaScript-Dateien nicht mehr antiquiert über Script-Tags einbinden zu müssen, bedient man sich eines Modul-Loaders. Hierfür gibt es eine Reihe von Formaten und Frameworks. Als Defakto-Standard sollte man das "**Asynchronous** Module Definition (AMD)"-Format [1] kennen. Die Referenzimplentierung von AMD wird durch das Framework require.js [2] gestellt. Sollte das das eigene Projekt sowohl AMD als auch CommonJS-Module benötigen, so hilft curl.js [3] aus der Misere.

AMD ist schnell erklärt, da man prinzipiell nur zwei globale Methoden benötigt: `define` und `require`. Wie der Name vermuten lässt, definiert `define` ein AMD-Modul.

##### Listing 2a -- Die Datei myFirstModule.js im AMD-Format
~~~~~
define(['jquery'], function($) {
    var result = function() {
        $('body').text('Hello World');
    }
    return result;
});
~~~~~

Idealerweise befindet sich in einer JavaScript-Datei auch immer nur ein AMD-Modul. Folgt man dieser Konvention, so kann man ein anonymes Modul erstellen. Hier ergibt sich der Name des Moduls aus dem geladenen Dateinamen mit Pfad - bei dem Groß- und Kleinschreibung zu beachten sind!

Mit `require` kann man dieses Modul wieder anfordern und dessen Rückgabewert weiter verwenden:

##### Listing 2b -- myFirstModule verwenden
~~~~~
require(['myFirstModule'], function (myFirstModule) {
    myFirstModule();
});
~~~~~ 
Der `require`-Befehl akzeptiert ein Array aus Modulnamen, welche alle vollständig geladen sein müssen, bevor die angegebene Callback-Funktion ausgeführt wird. Durch den Callback wird die Definition von Abhängigkeiten und deren tatsächliche Bereitstellung zeitlich voneinander getrennt und die gewünschte Asynchronität komfortabel zur Verfügung gestellt. Im vorliegenden Beispiel ist der Rückgabewert des Moduls eine einfache Funktion, welche "Hello World" im Browser ausgibt. Bemerkenswert ist die Tatsache, dass es für Verwender des Moduls nicht von Belang ist, welche weiteren Abhängigkeiten benötigt werden. Wie zu erkennen ist, hat das "myFirstModule" nämlich selbst eine Abhängigkeit zum Framework jQuery. Es ergibt sich ein Graph von Abhängigkeiten, welche require.js in der korrekten Reihenfolge auflösen wird. Viele Frameworks wie etwa jQuery, Underscore oder Knockout.js bringen AMD-Unterstützung bereits mit, andere Frameworks lassen sich durch ein wenig Konfiguration (so genannte "Shims") als Modul wrappen. Dank der breiten Unterstützung und der Möglichkeit von "Shims" kann man nun Objekten im globalen Gültigkeitsbereich (einer sehr schlechten Praxis) ganz und gar den Kampf ansagen und dennoch die Komplexität der Lösung gering halten.


#### AngularJS mit Require.js kombinieren
AMD-Module und Angular-Module sind somit zwei Konzepte, die unterschiedliche Schwerpunkte setzen. Mit ein paar kleinen Anpassungen lassen sich beide Welten kombinieren. 

Zuerst muss die Directive `ng-app` entfernt werden, da sonst das Bootstrapping zu früh beginnen würde. Man darf nicht mehr auf `DOMContentLoaded` warten, welches bereits dann feuern würde, wenn die wenigen synchron geladenen Scripte bereit stehen würden. Dies ist im folgenden Beispiel lediglich require.js selbst. Es wird weiterhin fast immer notwendig sein, ein paar Pfade anzupassen und Shims zu setzen. Dies erledigt man mit dem Befehl `requirejs.config`. Anschließend kann die AngularJs Anwendung mittels `require()` angefordert werden.

##### Listing 3 -- HelloWorld.cshtml wird um require.js ergänzt
~~~~~
<!DOCTYPE html>
<html>
<head>
    <title>Hello World AMD</title>
</head>
<body>

    <div ng-controller="exampleController">
        <h1 ng-bind="model.text"></h1>
    </div>

    <script src="Scripts/require.js"></script>
    <script>
        
        requirejs.config({
            baseUrl: '/Scripts',
            paths: {
                'jquery': 'jquery-2.1.1'
            },
            shim: {
                angular: {
                    exports: 'angular',
                    deps: ['jquery']
                }
            }
        });

        require(['examples/exampleApp']);
    </script>
</body>
</html>
~~~~~

Leider hat sich durch die Konfiguration und den `require`-Befehl die Anzahl der Codezeilen im Vergleich zu Listing 1 erhöht. Doch zum Glück unterstützt require.js die Angabe eines  einzigen Moduls direkt im script-Tag. Es bietet sich an, an dieser zentralen Stelle zunächst die Konfiguration selbst nachzuladen (hier "require.config" genannt) und anschließend die Anwendung anzufordern. So erhält man eine Lösung, die im Vergleich mit einer Zeile weniger auskommt.  

##### Listing 4a -- HelloWorld.cshtml refactored
~~~~~
<!DOCTYPE html>
<html>
<head>
    <title>Hello World AMD</title>
</head>
<body>

    <div ng-controller="exampleController">
        <h1 ng-bind="model.text"></h1>
    </div>

    <script src="Scripts/require.js" data-main="Scripts/init"></script>
</body>
</html>
~~~~~

##### Listing 4b -- Die Datei init.js im AMD-Format
~~~~~
require(['require', 'require.config'], function (require) {
    require(['examples/exampleApp']);
});
~~~~~

Es fehlt zur Vervollständigung des Beispiels jene Datei für die Anwendung selbst. Laut Quelltext ist diese auf dem Webserver unter dem Pfad "/Scripts/examples/examplesApp.js" aufrufbar, beinhaltet ein AMD-Modul mit dem Namen "examples/exampleApp" sowie darin enthalten ein AngularJS-Modul mit dem Namen "exampleApp". Wie Sie sehen, müssen Namen in den beiden Modul-Welten nicht überein stimmen. Es liegt an Ihnen, für die Benennung und Verzeichnisorganisation passende Konventionen zu finden.

##### Listing 4c -- Die Datei exampleApp.js im AMD-Format mit Angular-Modul
~~~~~
define(['require', 'angular'], function (require, angular) {

    var app = angular.module('exampleApp', [])
        .controller('exampleController', function ($scope) {

            $scope.model = {
                text: 'Hello World'
            }
        });

    // bootstrap Angular after require.js and DOM are ready
    angular.element(document).ready(function () {
        angular.bootstrap(document, ['app']);
    });

    return app;
});
~~~~~

Ungelöst ist immer noch das Bootstrapping, welches nicht mehr über `ng-app` realisiert werden kann. Da ab dem Require-Callback alle notwendigen Dateien geladen sind, ist es an der Zeit das AngularJS-Bootsrapping mittels `angular.bootstrap` zu beginnen. Et voilà - die ersten beiden Zutaten aus unserem Technologiemix sind angerichtet!

<hr>

## Aufgaben


1. Die Anwendung zeigt lediglich "HelloWorld" an. Definiere ein Array mit Kunden-Objekten. (Werte: CustomerId, FirstName, LastName, Mail)  
  Zeige diese Daten mittels ngRepeat [4] an!
2. Verwende ngClick [5] um den Namen eines Kunden zu ändern.  


<hr>

[1] AMD.Format: https://github.com/amdjs/amdjs-api/wiki/AMD    
[2] Require.js: http://requirejs.org/  
[3] Curls.js: https://github.com/cujojs/curl  
[4] ngRepeat: https://docs.angularjs.org/api/ng/directive/ngRepeat
[5] ngClick: https://docs.angularjs.org/api/ng/directive/ngClick

<hr>

&copy; 2015, Johannes Hoppe