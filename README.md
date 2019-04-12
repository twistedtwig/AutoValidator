# AutoValidator

## GOAL

AutoValidator's aim is to create a simple, fluent and intiutive framework to validate data and models in your dotnet applications. Simply put an application would validate some data and get a result object.  That object would state sucess or failure and detail the failing data.

There are two basic ways to validate data:

 * Simply call validate each item of data as you need to
 * Define a validation schema for a model then call validate on an instance of that model.

One of the principles AutoValidator follows is that exceptions should only happen in exceptional situations so it will not throw an exception when some data is incorrect, it will simply return a result object detailing what is wrong.

the validation schema process is heavily guided by [AutoMapper](https://github.com/AutoMapper/AutoMapper).

## Creating a validator

There are two ways to create a validator entity;

 1) Directly newing one: `var validator = new Validator();`
 2) Using the factory `IValidatorFactory` => `validatorFactory.Create();`

 ## How does the validation work?

 once you have an instance of a validator you can fluently apply all of your validation rules then finally call `Validate()` to get the validationResult object.

 ```
 validator
    .IsEmailAddress("myemail.com", "email")
    .NotNullOrEmpty("regCode", "registrationCode")
    .Validate();
 ```

 ## Validation methods:

 ```
IValidator IsEmailAddress(string email, string propName = "email", string message = null);
IValidator NotNullOrEmpty(string text, string propName, string message = null);
IValidator MinLength(string text, int minLength, string propName, string message = null);
IValidator MaxLength(string text, int maxLength, string propName, string message = null);
```

## Simple validation

The simplest way to validate is to call each validation method providing all the data required.

`validator.IsEmailAddress("myemail.com", "email").Validate();`

The example above validates the email address `myemail.com` for the property name of `email` and will use the default error message.  Lets break this down a little:

 * The fist property is what we want to validate, i.e. a value that has been passed back from the UI.
 * The second value is the name of the property you wish to validate.  For example in the model or UI it could be called, `email`, `password`, `confirmPassword`.  If there is an error it will be assigned to the value of this property.
 * the last and optional parameter is an error message.  If it is left blank the default will be used.  It uses a `string.Format` taking each of the parameters (excluding the first value, email in this case).  For example `MaxLength` has: `public static string MaxLength => "{1} should not be longer than {0}";` where `{1}` is `propName` and `{0}` is `maxLength`.

 ## Validation Schemas

 -------- todo write this up -------- 


### TODO
 - [] write up validation schemas
 - [] create configuration object for validation schema
 - [] creating maps for all functions
 - [] creating custom validation function
 - [] configuration setup to allow add mappings from assemblies
 - [] allow global CreateMap to be defined in initial setup configuration
 - [] ensure all mappings are being saved
 - [] factory for class type to get instance of a validator with the defined mappings
 - [] be able to use the mapping style in simple validation process

