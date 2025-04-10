mergeInto(LibraryManager.library, {
  doExit: function () {
    if(document.fullscreenElement){
      document.exitFullscreen();
    }else{
      location.reload();
    }
  },
  testParamCall: function (str) {
    window.alert(UTF8ToString(str));
  },
  ExitFullScreen: function () {
    if(document.fullscreenElement){
      document.exitFullscreen();
    }
  },
  Reload: function () {
    location.reload();
  },
  IsFullScreen: function () {
    return document.fullscreenElement!=null;
  },
});