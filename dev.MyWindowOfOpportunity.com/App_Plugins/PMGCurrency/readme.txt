Allow a flexible currency format for users to input money values.  You can specify options like which currency symbol to use, putting the symbol in front or back, which character to use for 1000's separation, which character to use for the decimal, and more.

This package implements the autoNumeric currency control from: https://github.com/BobKnothe/autoNumeric.  The value that is saved is the raw number without the formatting.

The directive has full coverage of the options, however I've noticed that perhaps not all of the options work or are a little confusing.  If you have questions about the options please see the following pages:

https://github.com/BobKnothe/autoNumeric
http://www.decorplanit.com/plugin/

If you have any issues,  please log them in the umbraco forum.

Notes:
    Creates a data type called Currency Input.  Add this to a document type.

    NONE of the options are required and the package will use the plugin defaults. 

    The value is stored as a string.  You may need to cast it if you are going to use it in calculations.

    Make sure to touch your webconfig/flush cache after installing so your umbraco can pick up the new property editor.

Credits:
  The autoNumeric plugin is being used from: https://github.com/BobKnothe/autoNumeric

  Icon: https://www.iconfinder.com/icons/134167/cash_currency_exchange_money_icon#size=128