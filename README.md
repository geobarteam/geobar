GoSimple helps you build loosely-coupled applications where you can swap out particular components without affecting the rest of the application.

The first library is: Go.Simple.Logging

GoSimple.Logging is a small and simple library that provides a common interface for logging.

It comes together with an implementation for Log4Net: GoSimple.Logging.Log4Net.

This library provides also 2 new appenders:

•	Syslog Appender: used to send your logs over TCP or UDP to a syslog server like Splunk, Logstach or Kiwi.

•	Rolling File Appender: when GoSimple rolls a log file, it saves and closes the old file and starts a new file. 

GoSimple.Logging comes with a sample project. It provides an example of how to configure your application for GoSimple.Logging in just one line of code. You’ll also find a sample Log4Net config file with example config sections for each appender.

To enable it on your project you need to :

1)	Review the Log4Net.config file on thr application root.

2)	In VS set the property “Copy to output directory” of the Log4Net.config file to “Copy always”

3)	In your application entry point (bootstrapper/main) initialize the Logger: 
Logger.Initialize(new Log4NetLogger());


Voilà that’s all you need to configure.
