/// <reference path="angular.js" />

var app = angular.module("myapp", ["ngRoute"]);

app.config(function($routeProvider,$locationProvider) {
    $routeProvider
    .when("/", {
        redirectTo: "/home",
        controller:"HomeController"
    })
    .when("/register", {
        templateUrl: "/Account/Register",
        controller: "RegisterController"
    })
    .when("/login", {
        templateUrl: "/Account/Login",
        controller: "LoginController"
    }).when("/messages", {
        templateUrl: "/Messages",
        controller: "MessagesController"
    });;
    $locationProvider.html5Mode(true);
});

app.controller("HomeController",function ($scope) {
    $scope.msg="Home Page"
})
app.controller("RegisterController", function ($scope) {
    $scope.msg = "Register Page"
})
app.controller("LoginController", function ($scope) {
    $scope.msg = "Login Page"
})
app.controller("MessagesController", ['$scope', '$http', function ($scope, $http) {
    $scope.Date = "Date";
    $scope.Id = "Id";
    $scope.SortBy = "";
    $scope.Sort = function (SortMethod) {
        let returnVal = SortMethod;
        if (SortMethod.charAt(0) != "-") {
            if (SortMethod == $scope.Id) {
                $scope.Id = "-" + $scope.Id;
            }
            else {
                $scope.Date = "-" + $scope.Date;
            }
        }
        else {
            if (SortMethod == $scope.Id) {
                $scope.Id = $scope.Id.slice(1);
            }
            else {
                $scope.Date = $scope.Date.slice(1);
            }
        }
        $scope.SortBy = SortMethod;
        console.log($scope.SortBy);
    }
    $scope.AllMessages = null;
    $scope.CurrentUserMessages = null;
    $http({
        url: "/Messages/GetAllMessages",
        method: "GET"
    }).then(function (require) {
        $scope.AllMessages = require.data;
        console.log($scope.AllMessages);
    });
    $http({
        url: "/Messages/GetCurrentUserMessages",
        method: "GET"
    }).then(function (require) {
        $scope.CurrentUserMessages = require.data;
        console.log($scope.CurrentUserMessages);
    });
    $scope.msg = "Messages Page";
    $scope.tabs = [{
        title: 'Write message',
        url: 'one.tpl.html'
    }, {
        title: 'User messages',
        url: 'two.tpl.html'
    }, {
        title: 'All messages',
        url: 'three.tpl.html'
    }];

    $scope.currentTab = 'one.tpl.html';

    $scope.onClickTab = function (tab) {
        $scope.currentTab = tab.url;
    }

    $scope.isActiveTab = function (tabUrl) {
        return tabUrl == $scope.currentTab;
    }

    $scope.message = "Insert meassage";
    $scope.SendMes = function (messageVal) {
        $http({
            url: "/Messages/SendMessage",
            method: "GET",
            params: { messageVal: messageVal }
        }).then(function (require) {
            $http({
                url: "/Messages/GetCurrentUserMessages",
                method: "GET"
            }).then(function (require) {
                $scope.CurrentUserMessages = require.data;
                console.log($scope.CurrentUserMessages);
            });
            $scope.AllMessages = require.data;
            console.log($scope.AllMessages);
        });
    }
    
    //$http.get("Messages/AllStudents")
    //.then(function (response) {
    //    $scope.students = response.data
    //})
}])