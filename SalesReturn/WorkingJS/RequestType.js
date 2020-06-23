var app = angular.module('RequestTypeModule', []);

app.service('RequestTypeService', function ($http, $location) {


    this.GetRequestTypeList = function () {

        return $http.get('api/RequestType/GetRequestType', {})

    };


    this.DeleteRequestType = function (Id, EmployeeCode) {

        return $http.get('api/RequestType/DeleteRequestType?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {})

    };

    this.SaveRequestType = function (list) {

        return $http.post('api/RequestType/SaveRequestType', list, {})

    };


});



app.controller('RequestTypeController', function ($scope, RequestTypeService, $rootScope) {
    console.log("RequestType");

    $scope.RequestTypeList = [];


    $scope.init = function () {
        RequestTypeService.GetRequestTypeList().then(function (success) {

            if (success.data != null) {
                $scope.RequestTypeList = success.data;

            }

        }, function (error) {

            console.log(error);

        });

    };

    $scope.Update = function () {
        console.log($scope.RequestTypeList);

        angular.forEach($scope.RequestTypeList, function (value, key) {
            value.EmployeeCode = $rootScope.session.EMP_CODE;
        });

        RequestTypeService.SaveRequestType($scope.RequestTypeList).then(function (success) {

            swal("Success", success.data, "success");
            $scope.init();

        }, function (error) {

            console.log(error);

        });

    };

    $scope.AddNewRow = function () {
        $scope.RequestTypeList.push({
            RequestType: ""
        });


        console.log($scope.RequestTypeList);
    };


    $scope.RemoveRow = function (Id, index) {


        if (Id) {
            swal({
                title: "Are you sure?",
                text: "You want to delete.",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
function () {

    RequestTypeService.DeleteRequestType(Id, $rootScope.session.EMP_CODE).then(function (success) {
        $scope.init();
        swal("Deleted!", success.data, "success");
    }, function (error) {

        console.log(error);

    });
});

        }
        else {
            $scope.RequestTypeList.splice(index, 1);
        }

        console.log($scope.RequestTypeList);
    };

    $scope.init();
});
