require './ruby/common.rb'
require './ruby/build.rb'
require './ruby/test'
require './ruby/msbuild'
  
#--Task dependencies
task :default => [:build]

