import 'package:fluro/fluro.dart';
import 'package:mymangawebsite/widgets/BodyMangaSite.dart';
import 'package:mymangawebsite/widgets/MangaGallery/MangaGalleryBody.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'main.dart';

class Flurorouter {
  static final FluroRouter router = FluroRouter();

  static final Handler _HomePage = Handler(
      handlerFunc:
          (BuildContext? context, Map<String, List<String>> parameters) =>
              MangaApp());

  static final Handler _galleryHandler = Handler(
      handlerFunc:
          (BuildContext? context, Map<String, List<String>> parameters) =>
              MangaGalleryBody(
                mangatype: parameters['manganame']![0],
                chapterlink: parameters['name']![0]
              )); // this one is for one paramter passing...

  // lets create for two parameters tooo...
  /*
  static final Handler _mainHandler2 = Handler(
      handlerFunc: (BuildContext context, Map<String, dynamic> params) =>
          MangaGalleryBody(page: params['name'][0],extra: params['extra'][0],));
*/
  // ok its all set now .....
  // now lets have a handler for passing parameter tooo....

  static void setupRouter() {
    router.define(
      '/',
      handler: _galleryHandler,
      transitionType: TransitionType.materialFullScreenDialog,
    );

    router.define(
      '/:manganame/:name',
      handler: _galleryHandler,
      transitionType: TransitionType.materialFullScreenDialog,
    );
  }
}
