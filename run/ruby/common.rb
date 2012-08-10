require 'rake'
require 'albacore'
require 'securerandom'
require 'fileutils'
require './ruby/gallio.rb'
require 'win32/registry'
  
PROGRAMM_FILES = ENV['ProgramFiles']
ROOT = File.join(Dir.pwd, "..")
SRC = File.join(ROOT, 'src')
BIN = File.join(ROOT, 'bin')
LIB = File.join(ROOT, 'lib')
CONFIG = File.join(ROOT, 'run', 'config')
 
FRAMEWORK = 'Wonga.QA.Framework'
TESTS = 'Wonga.QA.Tests'
SERVICETESTS = 'Wonga.QA.ServiceTests'
DATATESTS = 'Wonga.QA.DataTests'
UITESTS = 'Wonga.QA.UiTests'
MIGRATIONTESTS = 'Wonga.QA.MigrationTests'
TOOLS = 'Wonga.QA.Tools'
GENERATORS = 'Wonga.QA.Generators'
PREFIX = 'Wonga.QA'
 
 desc 'Reads the registry for the msbuildToolspath key'
def get_MSBuildToolsPath
  Win32::Registry::open(Win32::Registry::HKEY_LOCAL_MACHINE, 'SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\4.0') do |reg|
    type, value = reg.read('MSBuildToolsPath')
    value
  end
end

desc 'Returns all the files that match the given filename pattern in the BIN folder'
def find_dlls(mask)
  dlls = Dir.glob(File.join(BIN, "#{PREFIX}.#{mask}.dll"))
end

desc 'Retreives a list of dll paths for the given list of dll names/patterns. It searches in BIN'
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

def config(config_name)
   unless config_name.nil?
    cleanup
    if File.exists? File.join(CONFIG, "#{config_name}.v3qaconfig")
      FileUtils.cp File.join(CONFIG, "#{config_name}.v3qaconfig"), BIN
      puts "Configuring Test Target to be #{config_name}"
     
    else
      raise "#{config_name} config file not found"
    end
  end
end

def cleanup
  puts 'Cleaning up..'
  puts 'Deleting any v3qaconfig files within /bin..'
  config_files_to_delete = Dir.glob(File.join(BIN, "*.v3qaconfig"))
  FileUtils.rm config_files_to_delete
end

task :pre_test_cleanup do 
  cleanup
end
  
task :post_test_cleanup do #should be extended
  cleanup
end
  
task :config, :test_target do |t, args| #ready
  args.with_defaults(:test_target => ENV['test_target'])
  config args[:test_target]
end
  
desc 'Pregenerates the serializers of any dlls that require it'
task :pre_generate_serializers do #ready
  #skipping pre-generate for now.
  #puts 'Generating serializers'
  #sh File.join(LIB,'sgen','sgen.exe') + ' ' + File.join(BIN,'Wonga.QA.Framework.Cs.dll') + ' /force'
end
#--
  
desc 'Accepts a list of dlls land uses ILMerge to merge them making sure that Tests.Core is always added first'
def merge(test_dlls)
  core_tests = File.join(BIN, "#{TESTS}.Core.dll")
  exclude = core_tests
  exclude_dlls = exclude.split(' ')
  include_dlls = test_dlls - exclude_dlls

  command = '"' + File.join(PROGRAMM_FILES, 'Microsoft', 'ILMerge','ILMerge.exe') + '"'
  output_file = File.join(BIN, "#{TESTS}.dll")
  params = '/targetplatform:v4,'+get_MSBuildToolsPath
  params += " /out:#{output_file}"
  params += ' '+ core_tests
  include_dlls.each { |dll| params+= ' ' + dll }
  
  sh command + ' ' + params
  puts "Merged as #{output_file}"
end
  
desc 'Accepts a dll list and a test filter and runs the tests with theh Gallio runner'
def gallio(test_dlls, test_filter)
  g = Gallio.new
  puts "Gallio dlls: #{test_dlls}"
  if test_dlls and not test_dlls.empty?
    test_dlls.each { |dll| g.addTestAssembly(dll) }
  else g.addTestAssembly(File.join(BIN, "#{TESTS}.dll"))
  end
  g.filter = test_filter if test_filter and not test_filter.empty? 
  g.reportDirectory = File.join(BIN, "#{TESTS}.Report")
  g.reportNameFormat = "#{TESTS}.Report"
  g.addReportType("xml")
  begin
    g.run
  ensure
    convert_reports
  end
end

task :test, :include, :exclude, :test_filter do |t, args|
  args.with_defaults(:include => ENV['include'], :exclude => ENV['exclude'], :test_filter => ENV['test_filter'])
  test(args[:include], args[:exclude], args[:test_filter])
end

def test(include = '', exclude = '', test_filter = '')
  incl = get_dlls_list include
  excl = get_dlls_list exclude
  test_dlls = incl - excl
  merge test_dlls
  wonga_qa_tests_dll = Dir.glob(File.join(BIN, "#{TESTS}.dll"))
  # Rake.application.invoke_task("gallio[#{test_dlls}, #{fltr}]")
  gallio wonga_qa_tests_dll, test_filter
end

def convert_reports
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
  
#--

