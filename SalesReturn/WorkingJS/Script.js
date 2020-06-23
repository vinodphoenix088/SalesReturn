var app = angular.module('homeApp', ['ngRoute', 'SalesReturnModule', 'ApprovalMatrixModule', 'RequestTypeModule',
    'ngLoadingSpinner', 'ReasonMasterModule', 'ui.bootstrap', 'ngAnimate', '720kb.datepicker', 'PendingRequestModule',
    'RejectedRequestModule', 'InprocessRequestModule', 'ApprovedRequestModule', 'AdminMasterModule', 'ngSanitize', 'ui.select', 'kendo.directives',
    'ReasonModule', 'FlowMatrixModule', 'EmployeeModule', 'SRVCloserModule', 'DashboardModule', 'CatalystStatusModule', 'CSOReasonModule', 'PendingSRVCloserModule', 'MasterReporttModule', 'SavedRequestModule','ClosedRequestModule']);

app.config(function (uiSelectConfig) {
    uiSelectConfig.removeSelected = false;
});

app.factory('httpRequestInterceptor', function () {
    return {
        request: function (config) {
            var session = angular.fromJson(sessionStorage.getItem("app")) || {};
            //console.log(session);
            // config.headers['Authorization'] = session.userAuthKey;
            if (session != null) {
                return config;
            }
            sessionStorage.setItem("app", null);
            //window.location.assign('./login.html');
            ////console.log(config);
            return;
        },
        responseError: function (response) {
            if (response.status === 403 || response.status === 400) {
                var data = {};
                sessionStorage.setItem("app", null);
                // window.location.assign('./login.html');
            }
        }
    };
});

app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.config(function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Partial/Dashboard.html' ,
            controller: 'DashboardController'
        })
        .when('/Dashboard/', {
            templateUrl: 'Partial/Dashboard.html' ,
            controller: 'DashboardController'
        })
        .when('/ReqForm/', {
            templateUrl: 'Partial/SalesReturnForm.html',
            controller: 'SalesReturnController'
        }).when('/AppMatrix/', {
            templateUrl: 'Partial/ApprovalMatrix.html',
            controller: 'ApprovalMatrixController'
        }).when('/ReqType/', {
            templateUrl: 'Partial/RequestType.html',
            controller: 'RequestTypeController'
        }).when('/ReasonMaster/', {
            templateUrl: 'Partial/ReasonMaster.html',
            controller: 'ReasonMasterController'
        }).when('/PendingRequest/', {
            templateUrl: 'Partial/PendingRequest.html',
            controller: 'PendingRequestController'
        }).when('/RejectedRequest/', {
            templateUrl: 'Partial/RejectedRequest.html',
            controller: 'RejectedRequestController'
        }).when('/InprocessRequest/', {
            templateUrl: 'Partial/InprocessRequest.html',
            controller: 'InprocessRequestController'
        }).when('/ApprovedRequest/', {
            templateUrl: 'Partial/ApprovedRequest.html',
            controller: 'ApprovedRequestController'

        }).when('/AdminMaster/', {
            templateUrl: 'Partial/AdminMaster.html',
            controller: 'AdminMasterController'

        }).when('/srr/', {
            templateUrl: 'Partial/ReasonSaleRetrurn.html', 
            controller: 'ReasonController'

        }).when('/am/', {
            templateUrl: 'Partial/ReasonApprovalMatrix.html',
          

        }).when('/srrp/', {
            templateUrl: 'Partial/SRVCloser.html',
            controller: 'SRVCloserController'
            

        }).when('/dm/', {
            templateUrl: 'Partial/ReasonEmployeMaster.html',
            controller: 'EmployeeController'
           

        }).when('/sbc/', {
            
            templateUrl: 'Partial/SrvBillingClosure.html',


        }).when('/cs/', {
            
            templateUrl: 'Partial/CatalystStatus.html',
            controller: 'CatalystStatusController'

        }).when('/PBC/', {

            templateUrl: 'Partial/PendingBillingCloser.html',
            controller: 'PendingSRVCloserController'

        }).when('/fm/', {
            templateUrl: 'Partial/FlowMatrix.html',
            controller: 'FlowMatrixController'

        }).when('/SAPSR/', {
            templateUrl: 'Partial/CSOSubReason.html',
            controller: 'CSOReasonController'

        }).when('/MR/', {
            templateUrl: 'Partial/MasterReport.html',
            controller: 'MasterReportController'

        }).when('/STR/', {
            templateUrl: 'Partial/SaleTeamwiseReport.html'//,

        }).when('/SavedRequest/', {
            templateUrl: 'Partial/SavedRequest.html',
            controller: 'SavedRequestController'
        }).when('/ClosedRequest/', {
            templateUrl: 'Partial/ClosedRequests.html',
            controller: 'ClosedRequestController'
        }).otherwise({
            redirectTo: "/"
        });

    $locationProvider.hashPrefix('');
    //$httpProvider.interceptors.push('httpRequestInterceptor');
});

app.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});

app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

app.service('HomeService', function ($http, $location) {

    this.Request_Id = 0

    this.AppUrl = "";

    //console.log($location.absUrl());

    if ($location.absUrl().indexOf('SalesReturn') != -1) {

        this.AppUrl = "/SalesReturn/api/"

    }
    else {

        this.AppUrl = "/api/"
    }

    this.GetUserDetails = function (id) {
        return $http.get(this.AppUrl + 'Login/GetUserDetails/?id=' + id);
    };

    this.GetEmployeeRequestDetails = function (id) {
        return $http.get(this.AppUrl + 'Common/GetEmployeeRequestDetails?RequestId=' + id);
    };
    this.GetRequestStatusDetails = function (id) {
        return $http.get(this.AppUrl + 'Common/GetRequestStatusDetails?RequestId=' + id);
    };
    this.GetFutureStatus = function (id) {
        return $http.get(this.AppUrl + 'Common/GetFutureStatus?RequestId=' + id);
    };
    this.GetCurrentStatus = function (id) {
        return $http.get(this.AppUrl + 'Common/GetCurrentStatus?RequestId=' + id);
    };

});

app.controller('HomeController', function ($scope, HomeService, $rootScope, $q, $location) {
    //console.log("homecontroller ");
    
    //-----------------------------------------------
    $rootScope.Maininit = function () {
        var deferred = $q.defer();
        var array = location.search.substring(1).split('=');
        var KEy = '';
        var Value = '';
        if (array.length > 0) {
            KEy = array[0];
            Value = array[1];
        }
        if (KEy == 'udi') {
            $rootScope.uid = Value;
            //console.log("$rootScope.uid", $rootScope.uid)
        }
        else {
            if ($location.search().hasOwnProperty('udi')) {
                uid = $location.search()['udi'];
                //$rootScope.uid = uid;
                window.location.assign('./index.html?udi=' + uid);
            }
        }
        //-------------------------------------
        console.log($rootScope.session);
        //console.log($rootScope.uid);
        if (!$rootScope.session) {
            HomeService.GetUserDetails($rootScope.uid).then(function (retdata) {
                //console.log(retdata);
                if (retdata == null) { swal("Login with valid  user Id"); return false; }

                if (angular.isObject(retdata)) {
                    ////console.log(retdata);
                    sessionStorage.setItem("app", angular.toJson(retdata.data));

                    var session = angular.fromJson(sessionStorage.getItem("app")) || {};
                    HomeService.UserDeatils = retdata;
                    //$location.search('udi', null);
                    //$location.url($location.path());
                    // window.location.assign('./guidelines.html');
                    $rootScope.session = angular.fromJson(sessionStorage.getItem("app")) || {};
                    //console.log($rootScope.session.Emp_First_name);
                    //console.log($rootScope.session.EMP_CODE);
                    if (angular.isObject($rootScope.session)) {
                        //if ($rootScope.session.Emp_First_name == null || $rootScope.session.EMP_CODE == null) { swal("Login First to continue "); return false; }
                        $scope.LoggedInUser = $rootScope.session.Emp_First_name;
                        $scope.RoleName = $rootScope.session.RoleName;
                        $scope.Employee_ID = $rootScope.session.EMP_CODE;
                        $scope.Desg_Desc = $rootScope.session.Desg_Desc;
                        //console.log($scope.LoggedInUser);

                    }
                    deferred.resolve($rootScope.session = session);
                }
            }, function (error) {

                deferred.reject($rootScope.session = {});
            });
        }
        else {
            deferred.resolve($rootScope.session);
        }

        return deferred.promise;
    }
    //----------------------------------------


    $rootScope.Maininit();
});






app.controller('ModalInstanceCtrl_StatusPending', function ($scope, $uibModalInstance, HomeService) {
    $scope.PendingStatusData = {};
    $scope.grid = {};
    
    //console.log("PendingRequestService.RequestID", HomeService.RequestID);


    $scope.Request_Id = HomeService.RequestID;
    $scope.Request_Option = HomeService.requestTypeoption;
    //console.log("Request_Id", $scope.Request_Id);
    $scope.initStatus = function () {

        $scope.EmployeeDetails = [];


        HomeService.GetEmployeeRequestDetails(HomeService.RequestID).then(function success(retdata) {
            if (retdata != null) {
                //console.log(retdata);
                $scope.EmployeeDetails = retdata.data;
            }
            else {
                //console.log('Error case');

            }
        }, function error(retdata) {
            //console.log(retdata);
        });
        var neeObject = {};

        HomeService.GetRequestStatusDetails(HomeService.RequestID).then(function success(retdata) {
            //debugger
            if (retdata != null) {
                $scope.grid = retdata.data;
            }
            else {
                //console.log('Error case');


            }

        }, function error(retdata) {

        });

        HomeService.GetFutureStatus(HomeService.RequestID).then(function success(retdata) {
            //debugger
            if (retdata != null) {

                $scope.FutureStatus = retdata.data;
            }
            else {
                //console.log('Error case');

            }
        }, function error(retdata) {
            //console.log(retdata);
        });
        HomeService.GetCurrentStatus(HomeService.RequestID).then(function success(retdata) {
            //debugger
            if (retdata != null) {

                $scope.PendingStatusData = retdata.data;

            }
            else {
                //console.log('Error case');


            }
        }, function error(retdata) {
            ////console.log(FutureStat);

        });



    };


    //$scope.cancel = function () {        
    //    $uibModalInstance.dismiss('cancel');


    $scope.initStatus();

    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.close();
    };


});