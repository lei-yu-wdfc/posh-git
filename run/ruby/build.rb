require 'albacore' 
require './ruby/common'

#-- build tasks for each solution  
msbuild :framework do |msb|
  msb.solution = File.join(SRC, FRAMEWORK,  "#{FRAMEWORK}.sln")
end
  
msbuild :tests do |msb|
  msb.solution = File.join(SRC, TESTS, "#{TESTS}.sln")
end
  
msbuild :service_tests do |msb|
  msb.solution = File.join(SRC, SERVICETESTS, "#{SERVICETESTS}.sln")
end
  
msbuild :data_tests do |msb|
  msb.solution = File.join(SRC, DATATESTS, "#{DATATESTS}.sln")
end

msbuild :ui_tests do |msb|
  msb.solution = File.join(SRC, UITESTS, "#{UITESTS}.sln")
end
  
msbuild :tools do |msb|
  msb.solution = File.join(SRC, TOOLS, "#{TOOLS}.sln")
end
#--

task :build => [:framework, :tests, :ui_tests, :service_tests, :data_tests, :tools]
#--

