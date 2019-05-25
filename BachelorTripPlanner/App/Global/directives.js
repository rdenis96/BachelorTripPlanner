globalModule.directive('homeInterestsSlider', function ($timeout, $window) {
    return {
        restrict: 'E',
        scope: {
            interests: '='
        },
        link: function (scope, elem, attrs) {
            scope.currentIndex = 0; // Initially the index is at the first interest

            scope.next = function () {
                scope.currentIndex < scope.interests.length - 1 ? scope.currentIndex++ : scope.currentIndex = 0;
            };

            scope.prev = function () {
                scope.currentIndex > 0 ? scope.currentIndex-- : scope.currentIndex = scope.interests.length - 1;
            };

            scope.openWikipediaTab = function (link) {
                $window.open(link, '_blank');
            };

            scope.$watch('currentIndex', function () {
                scope.interests.forEach(function (interest) {
                    interest.visible = false; // make every interest invisible
                });

                scope.interests[scope.currentIndex].visible = true; // make the current interest visible
            });

            var timer;
            var sliderFunc = function () {
                timer = $timeout(function () {
                    scope.next();
                    timer = $timeout(sliderFunc, 2000);
                }, 2000);
            };

            sliderFunc();

            scope.$on('$destroy', function () {
                $timeout.cancel(timer); // when the scope is getting destroyed, cancel the timer
            });
        },
        templateUrl: 'AppViews/Home/home-suggested-interests-slidetemplate.html'
    };
});