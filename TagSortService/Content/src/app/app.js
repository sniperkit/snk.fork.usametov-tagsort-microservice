
const tagBundleModule = angular.module("TagBundleUtil", []).controller
    ("tagBundleCtrl", ['$scope', '$location', '$window', 'tagRepository'
    , function ($scope, $location, $window, tagRepository) {

        const tagMover = function (x) {
            console.log("state in tag mover",$scope.state);
            $scope.state = editTagBundle($scope.state, Object.assign({ type: 'MOVE' }, x));
            $scope.$apply();
            if ($scope.state.val2focus)
                angular.element(x.srcId).val($scope.state.val2focus);
        };
        
        //hook tagmover routine to element's keyup event 
        const arrowKeyHandler = function (listIdSelector) {
            const srcList = $(listIdSelector);

            let arrowKeyUp = Rx.Observable.fromEvent(srcList, 'keyup')
                                          .filter(e=>e.keyCode === 37 || e.keyCode === 39)
                                          .map(function (e) {                                              
                                              return {
                                                  keyCode: e.keyCode.toString()
                                                  , srcId: listIdSelector
                                                  , slctValue: srcList.val()
                                              };
                                          });

            arrowKeyUp.subscribe(tagMover);
        };
           
        //state transitions are provided by $window service
        //keys in state transtions dictionary represent element ids (see select boxes below) 
        //to which we hook our keyup event handlers
        const setArrowKeyHandlers = function () {            
            //wire arrowKeyHandler to select boxes
            angular.forEach(Array.from($scope.state.states_transition_matrix.keys())
                                , function (selector) { arrowKeyHandler(selector); });
        };

        $scope.SaveTagBundleAndExcludeList = function () {
            resolvePromise(tagRepository.saveTagBundle
                            ($scope.state.selectedTagBundleId,
                             $scope.state.topTags,
                             $scope.state.exclTags,
                             $scope.state.exclTagBundles.split(','))
                           ,
                            function (response) {//
                               console.log("SaveTagBundleAndExcludeList, response status", response.status);
                           });
        };
           
    
        $scope.addEditTagBundleName = function () {
            if (typeof $scope.state.selectedTagBundleId === 'undefined'
                || $scope.state.selectedTagBundleId === 'new') {
                resolvePromise(tagRepository.createTagBundle
                                            ($scope.state.newTagBundleName, $scope.state.bookmarksCollectionId)
                               , function (response) {
                                   console.log("edit TagBundle Name, response status", response.status);
                                   $scope.ReloadPage();
                               });
            }
            else {
                resolvePromise(tagRepository.saveTagBundleName($scope.state.selectedTagBundleId, $scope.state.newTagBundleName)
                               , function (response) {
                                   console.log("saveTagBundleName, response status", response.status);                                   
                               });
            }
        };

        const resolvePromise = function (promise, successFn) {
            Rx.Observable.fromPromise(promise)
                        .subscribe(successFn, function (err) {
                            console.log('Error: %s, %s', err.status, err.statusText);
                            if (err.status == 401) {
                                //TODO: call reducer here
                                $scope.state.LoginRequired = true;
                            }
                        }
                        , null);
        };

        $scope.SetMostFrequentTags = function () {

            const bundleId = getSlctdTagBundleId($scope.state.selectedTagBundleId);            
            let promise = tagRepository.getMostFrequentTags
                                        (bundleId, $scope.state.exclTagBundles, $scope.state.buffer_size);

            resolvePromise(promise, function (response) {
                console.log("SetMostFrequentTags status", response.status);
                $scope.state.freqTags = response.data;                
                $scope.$apply();
            });
        };
   
        $scope.SetTagAssociations = function () {

            const bundleId = getSlctdTagBundleId($scope.state.selectedTagBundleId);
            let promise = tagRepository.getTagAssociations(bundleId, $scope.state.buffer_size);

            resolvePromise(promise, function (response) {
                $scope.state.freqTags = response.data;//using same array for associated terms
                //console.log("associated tags", response.data);
                $scope.$apply();
            });
        };
    
        $scope.ReloadPage = function (selectedTagBundleId) {
            //set url    
            if (selectedTagBundleId) {
                $location.search({
                    tagBundle: selectedTagBundleId
                , bookmarksCollectionId: $scope.GetBookmarksCollection()
                });
            }
            else {
                $location.search({ bookmarksCollectionId: $scope.GetBookmarksCollection() });
            }

            $window.location.reload();
        };

        const getSlctdTagBundleId = function (bundleId) {

            return $location.search()['tagBundle'] || bundleId;
        };

        $scope.GetBookmarksCollection = function () {

            return $location.search()['bookmarksCollectionId'];
        };

        $scope.InitPage = function (funcArray) {

            let bookmarksCollectionId = $scope.GetBookmarksCollection();
            if (typeof bookmarksCollectionId === "undefined") {
                alert("bookmarksCollectionId is undefined!");
                return;
            }

            $scope.state.bookmarksCollectionId = bookmarksCollectionId;

            let promise = tagRepository.getTagBundles(bookmarksCollectionId);

            resolvePromise(promise, function (response) {

                const slctBundleId = getSlctdTagBundleId(response.data ? response.data[0].id : null);                
                $scope.state.existingTagBundles = response.data; //console.log("tag bundles", response.data);                
                $scope.state.selectedTagBundleId = slctBundleId;                                
                $scope.state.LoginRequired = false;

                angular.forEach(funcArray, function (func) {
                    func(slctBundleId);
                });

                $scope.$apply();
            });
        };
                
        $scope.SetTagBundle = function (bundleId) {

            let promise = tagRepository.getTagBundleById(bundleId);

            resolvePromise(promise, function (response) {
                
                $scope.state = editTagBundle($scope.state, { type: 'SET_TAG_BUNDLE', data: response.data })//console.log("scope", $scope);
                $scope.$apply();
            });
        };
                
        $scope.InitFreqTagsModel = function () {            
            $scope.state = editTagBundle($scope.state, { type: 'INIT_FREQ_TAGS_MODEL' });
            setArrowKeyHandlers();

            $scope.InitPage
            ([
                $scope.SetTagBundle
            ]
            );
        };

        $scope.InitAddEditTagBundle = function () {
            $scope.state = editTagBundle({}, { type: 'INIT_ADD_EDIT_TAG_BUNDLE' });
            $scope.InitPage([]);
        };

        

    }]).factory("tagRepository", ['$http', function ($http) {
       
        $http.defaults.headers.common = {};
        $http.defaults.headers.common["Content-Type"] = "application/json";
        
        $http.defaults.headers.post = {};
        $http.defaults.headers.put = {};
        $http.defaults.headers.patch = {};

        const baseUrl = "http://localhost:55865/";
        
        const getTagBundleById = function (bundleId) {
            
            let promise = $http({
                url: baseUrl+"tagBundle/" + bundleId
                , method: "GET"
                , withCredentials: true
            });

            return promise;
        };

        const getTagBundles = function (bookmarksCollectionId) {
            
            let promise = $http({
                url: baseUrl+"tagBundleNames/"+(bookmarksCollectionId || "undefined"),
                method: "GET"
            });

            return promise;
        };

        const getMostFrequentTags = function (bundleId, excludeTagBundleNames, bufferSize) {
                            
            const xTagBundles = excludeTagBundleNames || "undefined";
            const url = baseUrl +
                    "NextMostFrequentTags/"+ bundleId +                    
                    "/" + bufferSize +
                    "/" + xTagBundles;
            
            let promise = $http({
                url: url,
                method: "GET"
            });

            return promise;                
        };

        const getTagAssociations = function (tagBundleId, bufferSize) {
            let promise = $http({
                url: baseUrl + "AssociatedTerms/" + tagBundleId + "/" + bufferSize,
                method: "GET"
            });

            return promise;
        };
      
        const saveTagBundle = function (tagBundleId, topTags, exclTags, exclTagBundles) {

            const tagBundle =
                   {
                       "Id": tagBundleId
                     , "Tags": topTags
                     , "ExcludeTags": exclTags
                     , "ExcludeTagBundles": exclTagBundles
                   };

            //console.log('tagBundle to save', tagBundle);

            let promise = $http({
                url: baseUrl + "tagBundle/updateById",
                method: "POST",
                data: tagBundle
            });

            return promise;
        };
        
        const createTagBundle = function (tagBundleName, bookmarksCollectionId) {

            const tagBundle = {
                "Name": tagBundleName
                ,
                "BookmarksCollections": [bookmarksCollectionId]
            };

            console.log('tagBundle to create', tagBundle);

            let promise = $http({
                url: baseUrl + "tagBundle/create",
                method: "POST",
                data: tagBundle
            });

            return promise;
        };

        const saveTagBundleName = function (bundleId, newTagBundleName) {

            const tagBundle = {
                "Name": newTagBundleName
                        ,
                "Id": bundleId
            };

            let promise = $http({
                url: baseUrl + "tagBundle/editName",
                method: "POST",
                data: tagBundle
            });

            return promise;
        };

        const tagService = {
            getTagBundleById: getTagBundleById,
            getTagBundles: getTagBundles,
            getMostFrequentTags: getMostFrequentTags,
            getTagAssociations: getTagAssociations,                        
            saveTagBundle: saveTagBundle,
            createTagBundle: createTagBundle,
            saveTagBundleName: saveTagBundleName
        };
        
        return tagService;
    }]);

tagBundleModule.controller("bookmarksCtrl", ['$scope', '$location', '$window', 'bookmarkRepository'
                                  , function ($scope , $location  ,  $window ,  bookmarkRepository) {

                                      const resolvePromise = function (promise, successFn) {
                                          Rx.Observable.fromPromise(promise)
                                                      .subscribe(successFn, function (err) {
                                                          console.log('Error: %s, %s', err.status, err.statusText);
                                                          if(err.status == 401){
                                                              $scope.state.LoginRequired = true;
                                                          }
                                                      }
                                                      , null);
                                      };

    let promise = bookmarkRepository.getBookmarkCollections();

    resolvePromise(promise, function (response) {

        $scope.state.bookmarkCollections = response.data;
        //console.log("bookmarkCollections", response.data);   
        $scope.state.bookmarksCollectionId = response.data[0].id;
        $scope.$apply();
    });

    $scope.ReloadBookmarkCollection = function (bookmarksCollectionId) {
        //console.log("bookmarksCollectionId", bookmarksCollectionId);
        if (bookmarksCollectionId) {
            $location.search({ bookmarksCollectionId: bookmarksCollectionId });
            $window.location.reload();
        }
    };

}]).factory('bookmarkRepository', ['$http', function ($http) {

    $http.defaults.headers.common = {};
    $http.defaults.headers.common["Content-Type"] = "application/json";
    
    $http.defaults.headers.post = {};
    $http.defaults.headers.put = {};
    $http.defaults.headers.patch = {};

    const baseUrl = "http://localhost:55865/";

    const getBookmarkCollections = function () {

        let promise = $http({
            url: baseUrl + "bookmarkCollections/",
            method: "GET"
        });

        return promise;
    };

    const bookmarkService = {
        getBookmarkCollections: getBookmarkCollections
    };

    return bookmarkService;

}]).config(['$httpProvider', function($httpProvider) {
    
    $httpProvider.defaults.headers.common = {};
    $httpProvider.defaults.headers.post = {};
    $httpProvider.defaults.headers.put = {};
    $httpProvider.defaults.headers.patch = {};
}]);

