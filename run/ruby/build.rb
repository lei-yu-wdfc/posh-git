require 'albacore' 
require './ruby/common'

#-- build tasks for each solution  
desc 'Builds the framework'
msbuild :framework do |msb|
  msb.solution = File.join(SRC, FRAMEWORK,  "#{FRAMEWORK}.sln")
end
  
 desc 'Builds the backend tests'
msbuild :tests do |msb|
  msb.solution = File.join(SRC, TESTS, "#{TESTS}.sln")
end
  
 desc 'Builds the service tests'
msbuild :service_tests do |msb|
  msb.solution = File.join(SRC, SERVICETESTS, "#{SERVICETESTS}.sln")
end
  
  desc 'Builds the data_tests'
msbuild :data_tests do |msb|
  msb.solution = File.join(SRC, DATATESTS, "#{DATATESTS}.sln")
end

desc 'Builds the ui_tests'
msbuild :ui_tests do |msb|
  msb.solution = File.join(SRC, UITESTS, "#{UITESTS}.sln")
end

desc 'Builds the migration_tests'
msbuild :migration_tests do |msb|
  msb.solution = File.join(SRC, MIGRATIONTESTS, "#{MIGRATIONTESTS}.sln")
end

desc 'Builds the tools'  
msbuild :tools do |msb|
  msb.solution = File.join(SRC, TOOLS, "#{TOOLS}.sln")
end

desc 'Builds the generators'
msbuild :generators do |msb|
  msb.solution = File.join(SRC, GENERATORS, "#{GENERATORS}.sln")
end
#--

desc 'Builds everything'
task :build => [:framework, :tests, :ui_tests, :migration_tests, :service_tests, :data_tests, :tools, :generators, :pre_generate_serializers]
#--

