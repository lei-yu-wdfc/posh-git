require 'rake'
require 'albacore'
require 'securerandom'
require 'fileutils'
require './ruby/gallio.rb'
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
PREFIX = 'Wonga.QA'
 
def get_MSBuildToolsPath
  Win32::Registry::open(Win32::Registry::HKEY_LOCAL_MACHINE, 'SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\4.0') do |reg|
    type, value = reg.read('MSBuildToolsPath')
    value
  end
end

def find_dlls(mask)
  Dir.chdir(BIN)
  dlls = Dir.glob("#{PREFIX}.#{mask}.dll")
end

def get_dlls_list(names)
  if names and not names.empty?
    list = Array.new
    names_list = names.split(':')
    names_list.uniq!
    names_list.each do |name| 
      dlls = find_dlls(name)
      list += dlls
    end
  else  
    list = Array.new
  end
  list
end

task :pre_test_cleanup do 
  bin_dir = Dir.chdir(BIN)
  config_files_to_delete = Dir.glob("*.v3qaconfig")
  FileUtils.rm config_files_to_delete
  puts 'cleanup'
end
  
task :post_test_cleanup do #should be extended
  bin_dir = Dir.new(BIN)
  config_files_to_delete = bin_dir.glob("*.v3qaconfig")
  FileUtils.rm config_files_to_delete
end
  
task :config, :test_target do |t, target| #ready
  target.with_defaults(:test_target => ENV['test_target'])
  Rake::Task[:pre_test_cleanup].invoke
  FileUtils.cp File.join(CONFIG, "#{target[:test_target]}.v3qaconfig"), BIN
  puts "test target: #{target[:test_target]}"
end
  
task :pre_generate_serializers do #ready
  puts 'Generating serializers'
  sh File.join(LIB,'sgen','sgen.exe') + ' ' + File.join(BIN,'Wonga.QA.Framework.Cs.dll') + ' /force'
end
#--
  
task :merge, :test_dlls do |t, args|#ready
  args.with_defaults(:test_dlls => ENV['test_dlls'])
  exclude = "#{TESTS}.Core.dll"
  exclude_dlls = exclude.split(' ')
  test_dlls = args[:test_dlls]
  include_dlls = test_dlls - exclude_dlls

  command = '"' + File.join(PROGRAMM_FILES, 'Microsoft', 'ILMerge','ILMerge.exe') + '"'
    
  params = '/targetplatform:v4,'+get_MSBuildToolsPath
  params += ' /out:' + File.join(BIN, "#{TESTS}.dll")
  params += ' '+ File.join(BIN, "#{TESTS}.Core.dll")
  include_dlls.each { |dll| params+= ' ' + dll }
  
  sh command + ' ' + params
#  cmd = Exec.new
#  cmd.command = command
#  cmd.parameters = params
#  cmd.execute
  puts "Merged"
end
  
desc 'run Gallio'
task :gallio, :test_dlls, :test_filter do |t, args| 
  args.with_defaults(:test_dlls => ENV['test_dlls'], :test_filter=> ENV['test_filter'])
  g = Gallio.new
  dlls = args[:test_dlls]
  puts "Gallio dlls: #{dlls}"
  fltr = args[:test_filter]
  if dlls and not dlls.empty?
    dlls.each { |dll| g.addTestAssembly(dll) }
  else g.addTestAssembly(File.join(BIN, "#{TESTS}.dll"))
  end
  g.filter = fltr if fltr and not fltr.empty? 
  g.reportDirectory = File.join(BIN, "#{TESTS}.Report")
  g.reportNameFormat = "#{TESTS}.Report"
  g.addReportType("xml")
  begin
    g.run
  ensure
    Rake::Task[:convert_reports].invoke
  end
end

task :test, :include, :exclude, :test_filter do |t, args|
  args.with_defaults(:include => ENV['include'], :exclude => ENV['exclude'], :test_filter => ENV['test_filter'])
  incl = get_dlls_list args[:include]
  excl = get_dlls_list args[:exclude]
  fltr = args[:test_filter] if args[:test_filter] 
  test_dlls = incl - excl
  puts test_dlls
  Rake::Task[:merge].invoke(test_dlls)
  Dir.chdir(BIN)
  wonga_qa_tests_dll = Dir.glob("#{TESTS}.dll")
  # Rake.application.invoke_task("gallio[#{test_dlls}, #{fltr}]")
  Rake::Task[:gallio].invoke(wonga_qa_tests_dll, fltr)

end
  
task :convert_reports do
  puts 'Converting reports'
  command = File.join(BIN,'Wonga.QA.Tools.ReportConverter','Wonga.QA.Tools.ReportConverter.exe')
  params1 = '"' + File.join(BIN, "#{TESTS}.Report", "#{TESTS}.Report.xml\" ") 
  params1 += '"' + File.join(BIN, "#{TESTS}.Report", "#{TESTS}.Report.html\" ")
  params1 += 'html'
	
  params2 = '"' + File.join(BIN, "#{TESTS}.Report", "#{TESTS}.Report.xml\" ")
  params2 += '"' + File.join(BIN, "#{TESTS}.Report", "#{TESTS}.Report.csv\" ")
  params2 += 'csv'
	
  sh command + ' ' + params1
  sh command + ' ' + params2
end
  
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
  
#--Task dependencies
task :default => [:build, :pre_generate_serializers]
  
task :build => [:framework, :tests, :ui_tests, :service_tests, :data_tests, :tools]
#--

