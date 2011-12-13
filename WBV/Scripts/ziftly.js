
ziftly = function (fbaccessToken, gift_json) {
  /* -------------------------------------------- ------ -------------------------------------------- */
  /* -------------------------------------------- MODELS -------------------------------------------- */
  /* -------------------------------------------- ------ -------------------------------------------- */
  window.SendConfirmModel = Backbone.Model.extend({
    url: function () { return "/api/gift/send" }
    , validate: function (attrs) {
      return;
      if (!window.sprints8.isEmail(this.get("gift").recipient.email))
        return "email not correct";
    }
  });

  window.RedemptionGiftModel = Backbone.Model.extend({});


  window.FBFriend = Backbone.Model.extend({ defaults: function () { return { name: 'FBFriend 1' }; } });
  window.FBFriendList = Backbone.Collection.extend({
    model: FBFriend
    , initialize: function (auth_handler) { this.auth_handler = auth_handler }
    , sync: function (method, model, options) {
      if (method != 'read') {
        console.log("FU: FriendList: Anything but fetch is forbidden");
      } else {
        console.log("Fetch friends from facebook");
        this.auth_handler.getFriends(options.success);
      }
    }
  });



  window.GiftSuggestion = Backbone.Model.extend({
    defaults: function () { return { name: 'Gift 1' }; }
  });
  window.GiftSuggestionList = Backbone.Collection.extend({
    model: GiftSuggestion
    , url: function () { return '/api/giftsuggestions/'; }
    , sync: function (method, model, options) {
      if (method != 'read') {
        console.log("GiftSuggestionList: Anything but fetch is forbidden");
      } else {
        console.log("Dummy fetch gift Suggestions from Server");
        var giftlist = [];
        giftlist.push({ name: "Restaurant Ticket", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 1 });
        giftlist.push({ name: "Restaurant Ticket: Burger King", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 2 });
        giftlist.push({ name: "Restaurant Ticket: McDonalds", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 3 });
        giftlist.push({ name: "Restaurant Ticket: KFC", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 4 });
        giftlist.push({ name: "Restaurant Ticket: Due Forni", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 5 });
        giftlist.push({ name: "Restaurant Ticket: The Mexican", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 6 });
        giftlist.push({ name: "Restaurant Ticket: The Ballroom", picture:"http://stockfresh.com/files/c/ccaetano/x/60/475343_21460906.jpg", id: 7 });
        options.success(giftlist, 'success', {});
      }
    }
  });


  /* -------------------------------------------- ----- -------------------------------------------- */
  /* -------------------------------------------- VIEWS -------------------------------------------- */
  /* -------------------------------------------- ----- -------------------------------------------- */

  window.FriendView = Backbone.View.extend({
    tagName: "li"
    , template: _.template($('#friend-listitem-template').html())
    , events: { "click .listbutton": "selectFriend" }
    , initialize: function () { }
    , render: function () {
      $(this.el).html(this.template(this.model.toJSON()));
      return this.el;
    }
    , selectFriend: function (e) {
      this.model.set({ selected: true });
    }
  });
  window.FriendListView = Backbone.View.extend({
    el: $('#selectrcptpage')
    , events: {}
    , initialize: function () {
      this.model.bind('add', this.addOne, this);
      this.model.bind('reset', this.addAll, this);
      this.model.bind('destroy', this.removeItem, this);
      this.model.bind('change:selected', this.rcptSelected, this);
    }
    , render: function () {
      App.navigator.to(2);
    }
    , addOne: function (item) {
      var view = new FriendView({ model: item });
      $("#recipientlist-items").append(view.render());
    }
    , addAll: function () {
      var widgetlist = [];
      this.model.each(function (item) { widgetlist.push(new FriendView({ model: item }).render()); });
      $("#recipientlist-items").html(widgetlist);
    }
    , rcptSelected: function (itemmodel) {
      if (itemmodel.get("selected")) {
        itemmodel.unset("selected", { silent: true });
        this.trigger("RcptSelected", itemmodel);
      }
    }
  });


  window.GiftSuggestionView = Backbone.View.extend({
    tagName: "li"
    , template: _.template($('#gift-listitem-template').html())
    , events: { "click .listbutton": "selectGift" }
    , initialize: function () { }
    , render: function () {
      $(this.el).html(this.template(this.model.toJSON()));
      return this.el;
    }
    , selectGift: function (e) {
      this.model.set({ selected: true });
    }
  });
  window.GiftSuggestionListView = Backbone.View.extend({
    el: $('#selectgiftpage')
    , events: {}
    , initialize: function () {
      this.model.bind('add', this.addOne, this);
      this.model.bind('reset', this.addAll, this);
      this.model.bind('destroy', this.removeItem, this);
      this.model.bind('change:selected', this.itemSelected, this);
    }
    , render: function () {
      App.navigator.to(3);
    }
    , addOne: function (item) {
      var view = new GiftSuggestionView({ model: item });
      $("#giftsuggestionslist-items").append(view.render());
    }
    , addAll: function () {
      var widgetlist = [];
      this.model.each(function (item) { widgetlist.push(new GiftSuggestionView({ model: item }).render()); });
      $("#giftsuggestionslist-items").html(widgetlist);
    }
    , itemSelected: function (itemmodel) {
      itemmodel.unset("selected", { silent: true });
      this.trigger("GiftSelected", itemmodel);
    }
  });





  window.SendConfirmView = Backbone.View.extend({
    el: $('#sendconfirmpage')
    , events: { "click .sendgiftbutton": "sendGift" }
    , template: _.template($('#gift-display-template').html())
    , render: function () {
      this.$("#rcpt-emailaddress").val("");
      this.$("#gift-info").html(this.template(this.model.get("gift")));
      App.navigator.to(4);
    }
    , sendGift: function () {
      var _t = this, button = _t.$(".sendgiftbutton"), button_text = button.html()
      button.html('<img src="/Content/loader.gif" style="top: 4px;position: relative;"/>').addClass("buttonplaceholder").removeClass("sendgiftbutton button");

      this.model.get('gift').recipient.email = this.$("#rcpt-emailaddress").val();
      this.model.save({}, { error: function (model, error) {
        _t.$("#rcpt-emailaddress").after('<span class="error">' + error + '</span>')
      }, success: function () {
        $(".error", _t.$("#rcpt-emailaddress").parentNode).remove();
        button.html(button_text).addClass("sendgiftbutton button").removeClass("buttonplaceholder");
        listRouter.navigate("sent", true);
      }
      });
    }
  });

  window.SentSuccessView = Backbone.View.extend({
    el: $('#sentsuccesspage')
    , render: function (gift) {
      this.$(".recipientname").html(gift.recipient.name);
      this.$(".giftname").html(gift.product.name);
      App.navigator.to(5);
    }
  });

  window.RedeemView = Backbone.View.extend({
    el: $('#redeempage')
    , template: _.template($('#gift-redeem-display-template').html())
     , events: { "click .redeemgift": "redeemGift" }
    , initialize: function () { }
    , render: function () {
      var _t = this;
      this.$("#gift-redeem-info").html(this.template(this.model.get("gift")));
      App.auth_handler.verify_user(this.model.get("gift").recipient.facebook_id, {
        valid: function () {
          _t.$(".hideable:not(.hidden)").addClass("hidden");
          _t.$(".redeemgift").removeClass("hidden");
        }
        , invalid: function () {
          _t.$(".hideable:not(.hidden)").addClass("hidden");
          _t.$(".pleasegoaway").removeClass("hidden");
          App.removeGift();
        }
        , notlogged_in: function () {
          _t.$(".hideable:not(.hidden)").addClass("hidden");
          _t.$(".fbloginbutton").removeClass("hidden");
        }
      });
      App.navigator.to(6);
    }
    , redeemGift: function () {
      this.redemption = this.redemption || new RedemptionView(this.model.get("gift"));
      this.redemption.render();
    }
    , destroy: function () {
      if (this.redemption) this.redemption.destroy();
      this.model.unbind();
      delete this.model;
      this.model = null;
      this.el.unbind();
    }
  });

  window.RedemptionView = Backbone.View.extend({
    el: $('#redemptionpage')
    , initialize: function (gift) {
      var _t = this;
      this.gift = gift;
      $.ajax({
        type: "POST"
        , dataType: "json"
        , url: "/api/gift/redeem"
        , contentType: "application/json; charset=utf-8"
        , data: JSON.stringify({ gift: { token: gift.product.token} })
        , success: function (model, status, xhr) {
          _t.$(".couponcode").html(model.gift.redeem_token);
          App.removeGift();
          _t.$(".hideable.hidden").removeClass("hidden");
        }
      });
    }
    , render: function () {
      App.navigator.to(7);
    }
    , destroy: function () {
      delete this.gift;
      this.gift = null;
      this.el.unbind();
    }
  });
  /* -------------------------------------------- --- -------------------------------------------- */
  /* -------------------------------------------- APP -------------------------------------------- */
  /* -------------------------------------------- --- -------------------------------------------- */

  window.MainAppView = Backbone.View.extend({
    el: $("#root")
    , events: { "click .fbloginbutton": "fbLogin" }
    , gift: {}
    , initialize: function () {
      var _t = this;
      this.navigator = new sprints8.Navigator();
      this.auth_handler = new sprints8.FBAuthHandler("249207335140842", "fb-root", fbaccessToken);

      this.giftsuggestions = new GiftSuggestionListView({ model: new GiftSuggestionList({}) });
      this.giftsuggestions.model.fetch();
      this.giftsuggestions.bind("GiftSelected", this.giftSelected, this);

      this.friendsuggestions = new FriendListView({ model: new FBFriendList(this.auth_handler) });
      this.friendsuggestions.model.fetch();
      this.friendsuggestions.bind("RcptSelected", _t.friendSelected, _t);
    }
    , friendSelected: function (recipient_model) {
      this.gift.recipient = recipient_model.toJSON();
      listRouter.navigate("gift", true);
    }
    , giftSelected: function (product_model) {
      this.gift.product = product_model.toJSON();
      this.sendconfirm = new SendConfirmView({ model: new SendConfirmModel({ gift: this.gift }) });
      listRouter.navigate("send", true);
    }
    , showSuccess: function () {
      if (_.keys(this.gift).length > 0) {
        new SentSuccessView().render(this.gift);
        this.gift = {};
      } else {
        listRouter.navigate("home", true);
      }
    }
    , render: function () {
      if (this.auth_handler.isLoggedIn()) {
        this.friendsuggestions.render();
      } else {
        this.navigator.to(1);
      }
    }
    , removeGift: function () {
      if (this.redemption_view) this.redemption_view.destroy();
      this.redemption_view = null;
      gift_json = null;
    }
    , reset: function () {
      if (gift_json) {
        this.redemption_view = this.redemption_view || new RedeemView({ model: new RedemptionGiftModel({ gift: JSON.parse(gift_json) }) });
        this.redemption_view.render();
      } else {
        this.render();
      }
    }
    , fbLogin: function () {
      var _t = this;
      this.auth_handler.addFBDeferred(function () {
        FB.login(function (response) {
          _t.auth_handler.setFBUser(response.session);
          _t.reset();
        }, { perms: 'email, publish_stream, offline_access, user_birthday, friends_birthday' });
      });
    }
  });
  window.App = new MainAppView();

  ListRouter = Backbone.Router.extend({
    routes: { "home": "resetView"
            , "send": "sendView"
            , "gift": "giftView"
            , "sent": "sentView"
            , "friend": "selectFriendView"
            , "*other": "resetView"
    }
    , resetView: function () {
      App.reset();
    }
    , selectFriendView: function () {
      if (App.auth_handler.isLoggedIn()) App.friendsuggestions.render();
      else listRouter.navigate("home", true);
    }
    , giftView: function () {
      if (App.auth_handler.isLoggedIn() && App.gift.recipient) App.giftsuggestions.render();
      else listRouter.navigate("home", true);
    }
    , sendView: function () {
      if (App.sendconfirm) App.sendconfirm.render();
      else listRouter.navigate("home", true);
    }
    , sentView: function () {
      if (App.auth_handler.isLoggedIn()) App.showSuccess();
      else listRouter.navigate("home", true);
    }
  });
  listRouter = new ListRouter;
  Backbone.history.start();
};