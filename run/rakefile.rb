require 'rake'
require 'albacore'
require 'securerandom'
require 'fileutils'
require './ruby/gallio.rb'
require './ruby/msbuild_helper.rb'
require 'win32/registry'
  
Dir.chdir('..')
  
PROGRAMM_FILES = ENV['ProgramFiles']
ROOT = Dir.pwd
SRC = File.join(ROOT, 'src')
BIN = File.join(ROOT, 'bin')
LIB = File.join(ROOT, 'lib')
CONFIG = File.join(ROOT, 'run', 'config')
 
FRAMEWORK = 'Wonga.QA.Framework'
TESTS = 'Wonga.QA.Tests'
SERVICETESTS = 'Wonga.QA.ServiceTests'
DATATESTS = 'Wonga.QA.DataTests'
UITESTS = 'Wonga.QA.UiTests'
TOOLS = 'Wonga.QA.Tools'
 
task :pre_test_cleanup do 
  bin_dir = Dir.new(BIN)
  config_files_to_delete = Dir.glob("*.v3qaconfig")
  FileUtils.rm config_files_to_delete
  puts 'cleanup'
end
  
task :post_test_cleanup do #should be extended
  bin_dir = Dir.new(BIN)
  config_files_to_delete = bin_dir.glob("*.v3qaconfig")
  FileUtils.rm config_files_to_delete
end
  
task :config do |t, arg| #ready
    test_target = ENV['test_target']
    FileUtils.cp File.join(CONFIG, test_target,'.v3qaconfig'), BIN unless test_target.empty?
    puts "test target: #{test_target}"
end
  
task :pre_generate_serializers do #ready
  exec do |cmd|
    cmd.command = File.join(LIB,'sgen','sgen.exe')
    cmd.parameters = File.join(BIN,'Wonga.QA.Framework.Cs.dll') + ' /force'
  end
  puts 'Generating serializers'
end
#--
  
task :merge do #ready
  exclude = File.join(BIN, "#{TESTS}.Core.dll")
  exclude += ' ' + File.join(BIN, "#{TESTS}.Meta.dll")
  exclude += ' ' + File.join(BIN, "#{TESTS}.Ui.dll")
  exclude += ' ' + File.join(BIN, "#{TESTS}.Ui.Mobile.dll")
  exclude += ' ' + File.join(BIN, "#{TESTS}.Migration.dll")
  exclude_dlls = exclude.split(' ')
  
  Dir.chdir(BIN)

  include = Dir.glob(TESTS + '.*.dll')
  include.each { |item| item.insert(0, BIN + '\\')}
  include_dlls = include - exclude_dlls
    
  command = File.join(PROGRAMM_FILES, 'Microsoft', 'ILMerge','ILMerge.exe')
    
  params = '/targetplatform:v4,'+read_msbuild_property("MSBuildToolsPath")
  params += ' /out:' + BIN + '\\' + TESTS + '.dll'
  params += ' ' + BIN + '\\' + TESTS + '.Core.dll'
  include_dlls.each { |dll| params+= ' ' + dll }
        
  exec do |cmd|
    cmd.command = command
    cmd.parameters = params
  end
end
  
desc 'run Gallio'
gallio :test, :files, :filter do |g, fls, fltr| 
  if fls and not fls.empty?
    files = fls.split(';')
    Dir.chdir(BIN)
    files.uniq!
    files.each { |file| g.addTestAssembly(BIN + '\\' + TESTS + '.' + file + '.dll') }
  else g.addTestAssembly(BIN + '\\' + TESTS + '.dll')
  end

  g.filter = fltr if fltr and not fltr.empty? 
  g.reportDirectory = BIN + '\\' + TESTS + '.Report'
  g.reportNameFormat = TESTS + '.Report'
  g.addReportType("xml")
end

task :run_tests, :files, :filter do |t, fls, fltr|  #change this shit
  Rake::Task[:pre_test_cleanup].invoke
  Rake::Task[:test].invoke(fls, fltr)
  Rake::Task[:post_test_cleanup].invoke  
end
  
task :sanity_test => [:config, :build, :merge, :pre_generate_serializers] do 
        Rake::Task[:test].invoke('Meta', '')
        Rake::Task[:convert_reports].invoke
        Rake::Task[:test].invoke('','Category:CoreTest')
        Rake::Task[:convert_reports].invoke
end
  
task :convert_reports do
	command = BIN + '\\Wonga.QA.Tools.ReportConverter\\Wonga.QA.Tools.ReportConverter.exe'
	params1 = '"' + BIN + '\\' + TESTS + '.Report\\' + TESTS + '.Report.xml" '
	params1 += '"' + BIN + '\\' + TESTS + '.Report\\' + TESTS + '.Report.html" '
	params1 += 'html'
	
	params2 = '"' + BIN + '\\' + TESTS + '.Report\\' + TESTS + '.Report.xml" '
	params2 += '"' + BIN + '\\' + TESTS + '.Report\\' + TESTS + '.Report.csv" '
	params2 += 'csv'
	
	sh command + ' ' + params1
	sh command + ' ' + params2
end
  
#-- build tasks for each solution  
msbuild :framework do |msb|
  msb.solution = File.join(SRC, FRAMEWORK, FRAMEWORK + '.sln')
end
  
msbuild :tests do |msb|
  msb.solution = SRC + '\\' + TESTS + '\\' + TESTS + '.sln'
end
  
msbuild :service_tests do |msb|
  msb.solution = SRC + '\\' + SERVICETESTS + '\\' + SERVICETESTS + '.sln'
end
  
msbuild :data_tests do |msb|
  msb.solution = SRC + '\\' + DATATESTS + '\\' + DATATESTS + '.sln'
end

msbuild :ui_tests do |msb|
    msb.solution = SRC + '\\' + UITESTS + '\\' + UITESTS + '.sln'
end
  
msbuild :tools do |msb|
  msb.solution = SRC + '\\' + TOOLS + '\\' + TOOLS + '.sln'
end
#--
  
#--Task dependencies
task :default => [:config, :build, :merge, :pre_generate_serializers]
  
task :build => [:framework, :tests, :ui_tests, :service_tests, :data_tests, :tools]
#--

