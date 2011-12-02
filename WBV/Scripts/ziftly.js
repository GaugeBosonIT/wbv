
ziftly = function (fbaccessToken) {
  /* -------------------------------------------- ------ -------------------------------------------- */
  /* -------------------------------------------- MODELS -------------------------------------------- */
  /* -------------------------------------------- ------ -------------------------------------------- */
  window.SendConfirmModel = Backbone.Model.extend({ defaults: function () { return { name: 'no email' }; } });


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
        for (var i = 0; i < 10; i++) {
          giftlist.push({ name: "Gift from Server " + i, id: i });
        }
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
      App.navigator.to(3);
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
        this.trigger("RcptSelected", itemmodel);
        itemmodel.set({ selected: false }, { silent: true });
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
      App.navigator.to(2);
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
      this.trigger("GiftSelected", itemmodel);
      itemmodel.set({ selected: false }, { silent: true });
    }
  });





  window.SendConfirmView = Backbone.View.extend({
    el: $('#sendconfirmpage')
    , events: { "click .sendgiftbutton": "sendGift" }
    , render: function () {
      this.$("#rcpt-emailaddress").val("");
      App.navigator.to(4);
    }
    , sendGift: function () {
      console.log(this.model.gift);
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
      this.auth_handler = new sprints8.FBAuthHandler('249207335140842', "fb-root");

      this.giftsuggestions = new GiftSuggestionListView({ model: new GiftSuggestionList({}) });
      this.giftsuggestions.model.fetch();
      this.giftsuggestions.bind("GiftSelected", this.giftSelected, this);

      this.friendsuggestions = new FriendListView({ model: new FBFriendList(this.auth_handler) });
      this.friendsuggestions.model.fetch();
      this.friendsuggestions.bind("RcptSelected", _t.friendSelected, _t);

    }
    , friendSelected: function (recipient_model) {
      this.gift.recipient = recipient_model;
      this.sendconfirm = new SendConfirmView({ model: new SendConfirmModel({ gift: this.gift }) });
      listRouter.navigate("send", true);
    }
    , giftSelected: function (product_model) {
      this.gift.product = product_model;
      listRouter.navigate("friend", true);
    }
    , render: function () {
      if (this.auth_handler.isLoggedIn()) {
        this.giftsuggestions.render();
      } else {
        this.navigator.to(1);
      }
    }
    , reset: function () {
      this.render();
    }
    , fbLogin: function () {
      var _t = this;
      this.auth_handler.addFBDeferred(function () {
        FB.login(function (response) {
          _t.auth_handler.setFBUser(response.session);
          _t.render();
        }, { perms: 'email, publish_stream, offline_access, user_birthday, friends_birthday' });
      });
    }
  });
  window.App = new MainAppView();

  ListRouter = Backbone.Router.extend({
    routes: { "home": "resetView"
            , "send": "sendView"
            , "friend": "selectFriendView"
            , "*other": "resetView"
    }
    , resetView: function () {
      App.reset();
    }
    , selectFriendView: function () {
      App.friendsuggestions.render();
    }
    , sendView: function () {
      App.sendconfirm.render();
    }
  });
  listRouter = new ListRouter;
  Backbone.history.start();
};