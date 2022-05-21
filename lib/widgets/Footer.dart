import 'package:flutter/material.dart';

class FooterWebsite extends StatelessWidget {
  const FooterWebsite({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return  Container(
      color:  Color(0xFF1A1A1A),
      child:  Padding(
          padding: EdgeInsets.all(8.0),
          child: Text("All rights to the Authors ®",
            textAlign: TextAlign.center,
            style: TextStyle(foreground: Paint()..color = Colors.white),

          )),
    );
  }
}
