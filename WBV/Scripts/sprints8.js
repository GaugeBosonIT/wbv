if(typeof window.sprints8==="undefined")window.sprints8 = {}
_.extend(window.sprints8, {
  resizePopup: function (e) {
    var popup = $(".loadingpopup:not(hidden)")
        , bg = $(".loadingbackground")
        , m = Math.max;
    if (popup) popup.css({ top: m(0, (bg.height() - popup.height()) / 2) + "px"
              , left: m(0, (bg.width() - popup.width()) / 2) + "px"
    });
  }
  , show_msg: function (text) {
    $(".loadingpopup .content").html(text || "loading");
    $(".loadingbackground, .loadingpopup .closing_button").click(sprints8.hide_msg);
    $("body > .loading.hidden").removeClass("hidden");
    $(window).bind("resize", sprints8.resizePopup);
    sprints8.resizePopup();
  }
  , hide_msg: function () {
    $("body > .loading").addClass("hidden");
    $(".loadingbackground").add(".loadingpopup .closing_button").unbind();
    $(window).unbind("resize", sprints8.resizePopup);
  }
  , isEmail: function (email) { return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(email) }
  , Navigator: function () {
    this.to = function (n) {
      window.scrollTo(0, 1);
      if (n != this.current_page){
        var reverseClass = (this.current_page > n) ? " reverse" : ""
                  , pages = $("#root > .container")
                  , frompage = $(pages.get(this.current_page))
                  , topage = $(pages.get(n))
                  , activePageClass = "active-page"
                  , transEnd = function (e) {
                    frompage.removeClass(activePageClass).removeClass(" out slide " + reverseClass);
                    topage.removeClass("in slide" + reverseClass);
                    topage.unbind('webkitAnimationEnd');
                    topage.unbind('animationend');
                    topage.unbind('oAnimationEnd');
                  };
        if (window.support['cssTransitions']) {
          topage.bind('webkitAnimationEnd', transEnd, this);
          topage.bind('animationend', transEnd, this);
          topage.bind('oAnimationEnd', transEnd, this);
          frompage.addClass("slide out" + reverseClass);
          topage.addClass(activePageClass).addClass("slide in " + reverseClass);
          window.scrollTo(0, 1);
        } else {
          frompage.removeClass(activePageClass);
          topage.addClass(activePageClass);
        }
        this.current_page = n;
       }
    }
  }
  , ConfigStore: function (key) {
    this.key = "configStore";
    this.save = function (data) {
      localStorage.setItem(this.key, JSON.stringify(data))
    };
    this.get = function () {
      return JSON.parse(localStorage.getItem(this.key) || "{}");
    };
    this.delList = function (link) {
      var config = this.get();
      config.unique_hash[link] = false;
      config.lists = _.filter(config.lists, function (l) { return l.link != link });
      this.save(config);
    };
    this.addList = function (link, name, afterSave, context) {
      var config = this.get(), newentry;
      config.unique_hash = config.unique_hash || {};
      if (config.unique_hash[link]) {
        return;
      } else {
        newentry = { link: link, name: name };
        config.lists = config.lists || []
        config.lists.unshift(newentry);
        config.unique_hash[link] = true;
        this.save(config);
        if (afterSave) { afterSave.call(context || this, newentry) }
      }
    };
    this.getAllLists = function () {
      return (this.get() || {}).lists || [];
    };
  }

  , deferreds: function (doneFunc, context) {
    var deferreds = [], _t = this;
    this.doneFunc = doneFunc;
    this.add = function (f) {
      deferreds.push(f);
      _t.run(arguments);
    };
    this.run = function () {
      if (_t.doneFunc.apply(context)) {
        var i = 0; len = deferreds.length;
        for (; i < len; i++) {
          var f = deferreds.pop();
          f.apply(_t, arguments);
        }
      }
    };
  }

  , FBAuthHandler: function (app_id, fbRootNode, access_token) {
    this.fbDoneLoading = false;
    this.fbUserID = null;
    this.fbToken = access_token;
    this.fbFriends = null;
    this.isLoggedIn = function () {
      return this.fbToken && this.fbToken !== 'NOTOKEN';
    };
    var _t = this;
    var loadDefs = new sprints8.deferreds(function () { return this.fbDoneLoading }, this);
    this.addFBDeferred = loadDefs.add;
    this.runFBDeferred = loadDefs.run;

    var loginDefs = new sprints8.deferreds(function () { return this.fbDoneLoading && this.isLoggedIn }, this);
    this.addLoginDeferred = loginDefs.add;
    this.runLoginDeferred = loginDefs.run;

    this.getFriends = function (cb) {
      if (!_t.fbFriends) {
        this.addLoginDeferred(function () {
          FB.Data.query("SELECT uid, name, pic_square, birthday_date FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1 = me())").wait(function (results) {
            _t.fbFriends = results;
            if (cb) cb(results)
          });
        });
      }
      else if (cb) cb(_t.fbFriends);
    };
    this.sendUserToServer = function (response, session) {
      $.ajax({
        type: "POST"
        , dataType: "json"
        , url: "/api/facebook/user"
        , contentType: "application/json; charset=utf-8"
        , data: JSON.stringify({ session: session, profile: response })
      });
    }

    this.setFBUser = function (session) {
      this.fbUserID = session.uid;
      this.fbToken = session.access_token;
      this.getFriends();
      FB.api("/me", function (response) { _t.sendUserToServer(response, session) });
      this.runLoginDeferred();
    };

    this.verify_user = function (expected_user_id, params) {
      _t.addFBDeferred(function () {
        var valid_callback = params.valid
          , notlogged_in_callback = params.notlogged_in
          , invalid_callback = params.invalid;

        if (_t.isLoggedIn()) {
          FB.api("/me", function (response) {
            if (response.id === expected_user_id) valid_callback();
            else invalid_callback();
          });
        } else {
          notlogged_in_callback();
        }
      });
    };

    window.fbAsyncInit = function () {
      var channelUrl = document.location.protocol + '//' + document.location.host + "/Scripts/channel.html";
      FB.init({ appId: app_id, status: true, cookie: true, xfbml: false, channelUrl: channelUrl, oauth: false });
      _t.fbDoneLoading = true;
      var fb = FB.getAccessToken;
      if (_t.isLoggedIn()) {
        FB.getAccessToken = function () { return fb() || _t.fbToken }
        _t.runLoginDeferred();
      }
      _t.runFBDeferred();
    };
    var e = document.createElement('script');
    e.src = 'https://connect.facebook.net/en_US/all.js';
    e.async = true;
    document.getElementById(fbRootNode).appendChild(e);
  }
});