angular.module("umbraco")
    .controller("pmg.CurrencyCtrl",
    function ($scope, contentResource, angularHelper, editorState, $q,
        notificationsService, navigationService, entityResource, assetsService, $location) {

        
        $scope.loaded = false;
        assetsService.load(['/App_Plugins/PMGCurrency/currency.js']).then(function () {
            _.mixin({
            compactObject: function (o) {
                _.each(o, function (v, k) {
                    if (!v)
                        delete o[k];
                });
                return o;
             }
           });

            $scope.loaded = true;
            $scope.config = _.compactObject($scope.model.config);

            $scope.preVal = $scope.model.value;

            $scope.rawNumber = $scope.model.value;

            $scope.$watch('rawNumber', function (newVal) {
                if ($scope.rawNumber != $scope.model.value)
                    $scope.model.value = newVal;
            });
        });




    }).directive('pmgCurrency', ['angularHelper', function (angularHelper) {
        return {
            restrict: 'AE',
            scope: {
                'pmgCurrency': '=',
                'config': '=',
                'loaded': '='
            },
            link: function (scope, element, attr) {
                var init = false;

                scope.$watch('loaded', function (newVal) {
                  if (!init && newVal) {
                    $(element).autoNumeric('init', scope.config);
                    if (scope.pmgCurrency) $(element).autoNumeric('set', scope.pmgCurrency);

                    $(element).change(function () {

                        scope.$apply(function () {
                            scope.pmgCurrency = $(element).autoNumeric('get');
                        });
                    });

                    init = true;
                }
                });
              
            }
        }
    } ]);